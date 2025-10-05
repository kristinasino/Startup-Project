using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Shared.Entities.Enums;

namespace Web.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> collection, string orderByInfo, OrderEnum sort, bool secondOrder)
        {
            Type type = typeof(T);

            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            // use reflection (not ComponentModel) to mirror LINQ
            PropertyInfo pi = type.GetProperty(orderByInfo, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            string methodName = String.Empty;

            if (secondOrder && collection is IOrderedQueryable<T>)
                methodName = sort == OrderEnum.Asc ? "ThenBy" : "ThenByDescending";
            
            else
                methodName = sort == OrderEnum.Asc ? "OrderBy" : "OrderByDescending";
            

            //TODO: apply caching to the generic methodsinfos?
            return (IOrderedQueryable<T>)typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                              && method.IsGenericMethodDefinition
                              && method.GetGenericArguments().Length == 2
                              && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { collection, lambda });

        }
    }
}