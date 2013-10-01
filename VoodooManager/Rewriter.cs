using System;
using System.Linq;
using System.Linq.Expressions;
using DynamicProxy;
using Microsoft.CSharp.RuntimeBinder;

namespace Manager
{
    public class Rewriter : ExpressionVisitor
    {
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Expression.Lambda(typeof(Action<VoodooProxy>), Visit(node.Body), (ParameterExpression) Visit(node.Parameters.First()));
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {

            return Expression.Invoke();
        }
    }
}
