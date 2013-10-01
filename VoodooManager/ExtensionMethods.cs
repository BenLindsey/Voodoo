using System;
using System.Linq.Expressions;
using DynamicProxy;

namespace Manager
{
    public static class ExtensionMethods
    {
        public static Action<VoodooProxy> AllowProxyArgument<T>(this Expression<Action<T>> expression)
        {
            var rewriter = new Rewriter();

            var expr = rewriter.Visit(expression);

            return ((Expression<Action<VoodooProxy>>)expr).Compile();
        }
    }
}
