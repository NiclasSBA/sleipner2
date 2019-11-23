using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Sleipner.Cache.Netcore.Policies
{
    public interface IConfiguredMethod<T> where T : class
    {
        bool IsMatch(MethodInfo method, IEnumerable<object> arguments);
    }
}
