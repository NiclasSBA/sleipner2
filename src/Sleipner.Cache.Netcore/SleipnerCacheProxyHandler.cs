using Sleipner.Cache.Netcore.LookupHandlers.Async;
using Sleipner.Cache.Netcore.LookupHandlers.Sync;
using Sleipner.Cache.Netcore.Policies;
using Sleipner.Core.Netcore;
using Sleipner.Core.Netcore.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sleipner.Cache.Netcore
{
    public class SleipnerCacheProxyHandler<T> : IProxyHandler<T> where T : class
    {
        private readonly T _implementation;
        private readonly ICachePolicyProvider<T> _cachePolicyProvider;
        private readonly ICacheProvider<T> _cache;
        private readonly AsyncLookupHandler<T> _asyncLookupHandler;
        private readonly SyncLookupHandler<T> _syncLookupHandler;

        private readonly TaskSyncronizer _taskUpdateSyncronizer;

        public SleipnerCacheProxyHandler(T implementation, ICachePolicyProvider<T> cachePolicyProvider, ICacheProvider<T> cache)
        {
            _implementation = implementation;
            _cachePolicyProvider = cachePolicyProvider;
            _cache = cache;

            _asyncLookupHandler = new AsyncLookupHandler<T>(_implementation, _cachePolicyProvider, _cache);
            _syncLookupHandler = new SyncLookupHandler<T>(_implementation, _cachePolicyProvider, _cache);

            _taskUpdateSyncronizer = new TaskSyncronizer();
        }

        public async Task<TResult> HandleAsync<TResult>(ProxiedMethodInvocation<T, TResult> methodInvocation)
        {
            return await _asyncLookupHandler.LookupAsync(methodInvocation);
        }

        public TResult Handle<TResult>(ProxiedMethodInvocation<T, TResult> methodInvocation)
        {
            return _syncLookupHandler.Lookup(methodInvocation);
        }
    }
}
