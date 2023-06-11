using System.Linq.Expressions;

namespace Light.Entity {
    /// <summary>
    ///     扩展Linqstring字段Where条件组合
    /// </summary>
    internal class ParameterReplacer : ExpressionVisitor {
        public ParameterReplacer(ParameterExpression paramExpr) {
            ParameterExpression = paramExpr;
        }

        public ParameterExpression ParameterExpression { get; }

        public Expression Replace(Expression expr) {
            return Visit(expr);
        }

        protected override Expression VisitParameter(ParameterExpression p) {
            return ParameterExpression;
        }
    }

    /// <summary>
    ///     扩展 字段Where条件组合
    /// </summary>
    public static class PredicateExtend {
        public static Expression<Func<T, bool>> True<T>() {
            return f => true;
        }

        public static Expression<Func<T, bool>> False<T>() {
            return f => false;
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expLeft,
            Expression<Func<T, bool>> expRight) {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(expLeft.Body);
            var right = parameterReplacer.Replace(expRight.Body);
            var body = Expression.And(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expLeft,
            Expression<Func<T, bool>> expRight) {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(expLeft.Body);
            var right = parameterReplacer.Replace(expRight.Body);
            var body = Expression.Or(left, right);
            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }


        /// <summary>
        ///     使之支持Sql in语法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="query"></param>
        /// <param name="obj"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIn<T, TValue>(this IQueryable<T> query, Expression<Func<T, TValue>> obj,
            IEnumerable<TValue> values) {
            return query.Where(BuildWhereInExpression(obj, values));
        }

        private static Expression<Func<TElement, bool>> BuildWhereInExpression<TElement, TValue>(
            Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values) {
            var p = propertySelector.Parameters.Single();
            var enumerable = values as TValue[] ?? values.ToArray();
            if (!enumerable.Any())
                return e => false;

            var equals = enumerable.Select(value =>
                (Expression)Expression.Equal(propertySelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate(Expression.Or);

            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }
    }
}