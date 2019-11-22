using Sleipner.Core.Netcore.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sleipner.Core.Netcore
{
    public interface IProxyHandler<T> where T : class
    {
        Task<TResult> HandleAsync<TResult>(ProxiedMethodInvocation<T, TResult> methodInvocation);
        TResult Handle<TResult>(ProxiedMethodInvocation<T, TResult> methodInvocation);
    }
}
