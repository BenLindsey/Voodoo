using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dynamifier
{
    public static class ExtensionMethods
    {
        public static Action<dynamic> AllowDynamicArguments<T>(this Expression<T> expression)
        {
            return o => Console.WriteLine("Todo");
        }
    }
}
