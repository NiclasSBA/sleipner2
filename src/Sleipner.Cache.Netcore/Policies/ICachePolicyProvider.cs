using Sleipner.Core.Netcore.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Sleipner.Cache.Netcore.Policies
{
    public interface ICachePolicyProvider<T> where T : class
    {
        CachePolicy GetPolicy(MethodInfo methodInfo, IEnumerable<object> arguments);
        CachePolicy GetPolicy<TResult>(ProxiedMethodInvocation<T, TResult> invocation);
        CachePolicy RegisterMethodConfiguration(IConfiguredMethod<T> methodConfiguration);
        CachePolicy GetDefaultPolicy();
    }
}
