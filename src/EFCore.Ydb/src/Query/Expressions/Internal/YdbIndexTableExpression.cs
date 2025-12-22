using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.Ydb.Query.Expressions.Internal;

/// <summary>
/// Represents a table accessed via a secondary index in YDB.
/// Generates SQL: FROM TableName VIEW IndexName
/// </summary>
public class YdbIndexTableExpression(
    string name,
    string? schema,
    string indexName,
    string? alias)
    : TableExpressionBase(alias), IEquatable<YdbIndexTableExpression>
{
    private static ConstructorInfo? _quotingConstructor;

    /// <summary>
    /// Gets the table name.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the schema name (if any).
    /// </summary>
    public string? Schema { get; } = schema;

    /// <summary>
    /// Gets the secondary index name.
    /// </summary>
    public string IndexName { get; } = indexName;

    protected override Expression VisitChildren(ExpressionVisitor visitor)
        => Update(alias: Alias);

    public YdbIndexTableExpression Update(string? alias)
        => ReferenceEquals(alias, Alias)
            ? this
            : new YdbIndexTableExpression(Name, Schema, IndexName, alias);

    public override TableExpressionBase WithAlias(string? alias)
        => Update(alias);

    public override TableExpressionBase Clone(string? alias, ExpressionVisitor cloningExpressionVisitor)
        => new YdbIndexTableExpression(Name, Schema, IndexName, alias);

    protected override TableExpressionBase WithAnnotations(IReadOnlyDictionary<string, IAnnotation> annotations)
    {
        var clone = new YdbIndexTableExpression(Name, Schema, IndexName, Alias);
        foreach (var annotation in annotations)
        {
            clone.AddAnnotation(annotation.Key, annotation.Value);
        }
        return clone;
    }

    [Experimental("EF9100")]
    public override Expression Quote()
        => New(
            _quotingConstructor ??= typeof(YdbIndexTableExpression).GetConstructor(
                [typeof(string), typeof(string), typeof(string), typeof(string)])!,
            Constant(Name),
            Constant(Schema, typeof(string)),
            Constant(IndexName),
            Constant(Alias, typeof(string)));

    public override bool Equals(object? obj)
        => obj is YdbIndexTableExpression other && Equals(other);

    public bool Equals(YdbIndexTableExpression? other)
        => ReferenceEquals(this, other)
           || (other is not null
               && base.Equals(other)
               && Name == other.Name
               && Schema == other.Schema
               && IndexName == other.IndexName);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Name, Schema, IndexName);

    protected override void Print(ExpressionPrinter expressionPrinter)
    {
        if (!string.IsNullOrEmpty(Schema))
        {
            expressionPrinter.Append(Schema).Append(".");
        }

        expressionPrinter.Append(Name);
        expressionPrinter.Append(" VIEW ").Append(IndexName);

        if (Alias != null)
        {
            expressionPrinter.Append(" AS ").Append(Alias);
        }
    }

    public override string ToString()
        => $"{(!string.IsNullOrEmpty(Schema) ? Schema + "." : "")}{Name} VIEW {IndexName}{(Alias != null ? " AS " + Alias : "")}";
}

