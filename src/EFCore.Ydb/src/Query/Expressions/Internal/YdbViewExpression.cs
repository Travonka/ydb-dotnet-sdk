using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.Ydb.Query.Expressions.Internal;

public class YdbViewExpression(SqlExpression index)
    : SqlExpression(typeof(bool), null)
{
    public override Expression Quote() => throw new NotImplementedException();

    protected override void Print(ExpressionPrinter expressionPrinter) => throw new NotImplementedException();
    
    protected override Expression VisitChildren(ExpressionVisitor visitor) => throw new NotImplementedException();
    
    public override bool Equals(object? obj) => throw new NotImplementedException();
    
    public bool Equals(YdbViewExpression? other) => throw new NotImplementedException();
    
    public override int GetHashCode() => throw new NotImplementedException();
}
