using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.Ydb.Query.Expressions.Internal;

public class YdbViewExpression(string name,
    string? schema,
    string indexName,   
    string? alias)
    : TableExpressionBase(alias), IEquatable<YdbViewExpression>
{
    private static ConstructorInfo? _quotingConstructor;
    public string Name { get; } = name;
    public string? Schema { get; } = schema;
    public string IndexName { get; } = indexName;
    public override TableExpressionBase Clone(string? alias, ExpressionVisitor cloningExpressionVisitor) => 
        new YdbViewExpression(Name, Schema, IndexName, alias);

    public override TableExpressionBase WithAlias(string newAlias) => ReferenceEquals(newAlias, Alias) ?
     this : new YdbViewExpression(Name, Schema, IndexName, newAlias);

    public override bool Equals(object? obj)
        => obj is YdbViewExpression other && Equals(other);

    [Experimental("EF9100")]
    public override Expression Quote()
        => New(
            _quotingConstructor ??= typeof(YdbViewExpression).GetConstructor(
                [typeof(string), typeof(string), typeof(string), typeof(string)])!,
            Constant(Name),
            Constant(Schema, typeof(string)),
            Constant(IndexName),
            Constant(Alias, typeof(string)));

    protected override void Print(ExpressionPrinter expressionPrinter) {
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

    protected override TableExpressionBase WithAnnotations(IReadOnlyDictionary<string, IAnnotation> annotations) => throw new NotImplementedException();

    public bool Equals(YdbViewExpression? other) => ReferenceEquals(this, other)
                                                    || (other is not null
                                                        && base.Equals(other)
                                                        && Name == other.Name
                                                        && Schema == other.Schema
                                                        && IndexName == other.IndexName);

    protected override Expression VisitChildren(ExpressionVisitor visitor)
        => WithAlias(Alias);

    public override string ToString()
        => $"{(!string.IsNullOrEmpty(Schema) ? Schema + "." : "")}{Name} VIEW {IndexName}{(Alias != null ? " AS " + Alias : "")}";
}
