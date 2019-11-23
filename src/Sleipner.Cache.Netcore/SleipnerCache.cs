using Sleipner.Cache.Netcore.Policies;
using Sleipner.Core.Netcore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sleipner.Cache.Netcore
{
    public class SleipnerCache<T> where T : class
    {
        private readonly T _implementation;
        private readonly ICacheProvider<T> _cache;

        public readonly ICachePolicyProvider<T> CachePolicyProvider;
        private readonly IProxyHandler<T> _proxyHandler;

        internal IList<IConfiguredMethod<T>> ConfiguredMethods = new List<IConfiguredMethod<T>>();

        public SleipnerCache(T implementation, ICacheProvider<T> cache)
        {
            _implementation = implementation;
            _cache = cache;

            CachePolicyProvider = new BasicConfigurationProvider<T>();
            _proxyHandler = new SleipnerCacheProxyHandler<T>(_implementation, CachePolicyProvider, _cache);
        }

        public T CreateCachedInstance()
        {
            var sleipnerProxy = new SleipnerProxy<T>(_implementation);
            return sleipnerProxy.WrapWith(_proxyHandler);
        }

        public void Config(Action<ICachePolicyProvider<T>> expression)
        {
            expression(CachePolicyProvider);
        }
    }
}
