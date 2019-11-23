using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sleipner.Cache.Netcore;
using Sleipner.Cache.Netcore.Policies;

namespace Sleipner.Cache.Netcore.RedisSharp
{
    public class RedisProvider<T> : ICacheProvider<T> where T : class
    {
        public Task DeleteAsync<TResult>(System.Linq.Expressions.Expression<Func<T, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Model.CachedObject<TResult>> GetAsync<TResult>(Core.Netcore.Util.ProxiedMethodInvocation<T, TResult> proxiedMethodInvocation, CachePolicy cachePolicy)
        {
            throw new NotImplementedException();
        }

        public Task StoreAsync<TResult>(Core.Netcore.Util.ProxiedMethodInvocation<T, TResult> proxiedMethodInvocation, CachePolicy cachePolicy, TResult data)
        {
            throw new NotImplementedException();
        }

        public Task StoreExceptionAsync<TResult>(Core.Netcore.Util.ProxiedMethodInvocation<T, TResult> proxiedMethodInvocation, CachePolicy cachePolicy, Exception e)
        {
            throw new NotImplementedException();
        }
    }
}
