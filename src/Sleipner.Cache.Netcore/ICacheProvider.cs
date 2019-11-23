using Sleipner.Cache.Netcore.Model;
using Sleipner.Cache.Netcore.Policies;
using Sleipner.Core.Netcore.Util;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sleipner.Cache.Netcore
{
    public interface ICacheProvider<T> where T : class
    {
        Task<CachedObject<TResult>> GetAsync<TResult>(ProxiedMethodInvocation<T, TResult> proxiedMethodInvocation, CachePolicy cachePolicy);
        Task StoreAsync<TResult>(ProxiedMethodInvocation<T, TResult> proxiedMethodInvocation, CachePolicy cachePolicy, TResult data);
        Task StoreExceptionAsync<TResult>(ProxiedMethodInvocation<T, TResult> proxiedMethodInvocation, CachePolicy cachePolicy, Exception e);
        Task DeleteAsync<TResult>(Expression<Func<T, TResult>> expression);
    }
}
