using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.Ydb.Query.Expressions.Internal;

public class YdbViewExpression(string name,
    string? schema,
    string indexName,
    string? alias)
    : TableExpressionBase(alias), IEquatable<YdbIndexTableExpression>
{
    public string Name { get; } = name;
    public string? Schema { get; } = schema;
    public string IndexName { get; } = indexName;
    public override TableExpressionBase Clone(string? alias, ExpressionVisitor cloningExpressionVisitor) => throw new NotImplementedException();

    public override TableExpressionBase WithAlias(string newAlias) => throw new NotImplementedException();

    public override Expression Quote() => throw new NotImplementedException();

    protected override void Print(ExpressionPrinter expressionPrinter) => throw new NotImplementedException();

    protected override TableExpressionBase WithAnnotations(IReadOnlyDictionary<string, IAnnotation> annotations) => throw new NotImplementedException();

    public bool Equals(YdbIndexTableExpression? other) => throw new NotImplementedException();
    
    public YdbIndexTableExpression Update(string? alias)
        => ReferenceEquals(alias, Alias)
            ? this
            : new YdbIndexTableExpression(Name, Schema, IndexName, alias);

}
