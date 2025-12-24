using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EntityFrameworkCore.Ydb.Extensions;

public static class YdbQueryableExtensions
{
    private static readonly MethodInfo FromIndexMethodInfo =
        typeof(YdbQueryableExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .First(m => m is {Name: nameof(FromIndex), IsGenericMethod: true});
    
    public static IQueryable<T> FromIndex<T>(this IQueryable<T> source, string indexName)
        where T : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (string.IsNullOrWhiteSpace(indexName))
            throw new ArgumentException("Index name cannot be null or empty.", nameof(indexName));

        var method = FromIndexMethodInfo.MakeGenericMethod(typeof(T));

        return source.Provider.CreateQuery<T>(
            Expression.Call(
                method,
                source.Expression,
                Expression.Constant(indexName)));
    }
}

