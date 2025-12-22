using EntityFrameworkCore.Ydb.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.Ydb.Query.Internal;

/// <summary>
/// Extension methods for creating YDB index table expressions.
/// </summary>
public static class YdbIndexTableExpressionExtensions
{
    /// <summary>
    /// Creates a table expression that uses a secondary index.
    /// Generates SQL: FROM TableName VIEW IndexName
    /// </summary>
    /// <param name="sqlExpressionFactory">The SQL expression factory.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="indexName">The name of the secondary index.</param>
    /// <param name="schema">Optional schema name.</param>
    /// <param name="alias">Optional table alias.</param>
    /// <returns>A new YdbIndexTableExpression.</returns>
    public static YdbIndexTableExpression IndexTable(
        this ISqlExpressionFactory sqlExpressionFactory,
        string tableName,
        string indexName,
        string? schema = null,
        string? alias = null)
        => new YdbIndexTableExpression(tableName, schema, indexName, alias);
}

