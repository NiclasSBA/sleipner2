﻿using Sleipner.Cache.Netcore.Model;
using Sleipner.Cache.Netcore.Policies;
using Sleipner.Core.Netcore.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sleipner.Cache.Netcore.DictionaryCache
{
    public class DictionaryCacheProvider<T> : ICacheProvider<T> where T : class
    {
        private readonly IDictionary<string, DictionaryCacheItem> _cache = new ConcurrentDictionary<string, DictionaryCacheItem>();

        public async Task<CachedObject<TResult>> GetAsync<TResult>(ProxiedMethodInvocation<T, TResult> proxiedMethodInvocation, CachePolicy cachePolicy)
        {
            return await Task.Factory.StartNew(() =>
            {
                var hash = proxiedMethodInvocation.GetHashString();
                DictionaryCacheItem cachedObject;
                if (_cache.TryGetValue(hash, out cachedObject))
                {
                    if (cachedObject.ThrownException != null && cachedObject.Created.AddSeconds(cachePolicy.ExceptionCacheDuration) > DateTime.Now)
                    {
                        return new CachedObject<TResult>(CachedObjectState.Exception, cachedObject.ThrownException);
                    }
                    else if (cachedObject.ThrownException != null)
                    {
                        return new CachedObject<TResult>(CachedObjectState.None, null);
                    }

                    if (cachedObject.AbsoluteDuration.TotalSeconds > 0 && cachedObject.Created + cachedObject.AbsoluteDuration < DateTime.Now)
                    {
                        return new CachedObject<TResult>(CachedObjectState.None, null);
                    }

                    return new CachedObject<TResult>(cachedObject.IsExpired ? CachedObjectState.Stale : CachedObjectState.Fresh, (TResult)cachedObject.Object);
                }
                return CachedObject<TResult>.Empty();
            });
        }

        public async Task StoreAsync<TResult>(ProxiedMethodInvocation<T, TResult> proxiedMethodInvocation, CachePolicy cachePolicy, TResult data)
        {
            await Task.Factory.StartNew(() =>
            {
                var hash = proxiedMethodInvocation.GetHashString();
                var duration = TimeSpan.FromSeconds(cachePolicy.CacheDuration);
                var absoluteDuration = TimeSpan.FromSeconds(cachePolicy.MaxAge);

                _cache[hash] = new DictionaryCacheItem(data, duration, absoluteDuration);
            });
        }

        public async Task StoreExceptionAsync<TResult>(ProxiedMethodInvocation<T, TResult> proxiedMethodInvocation, CachePolicy cachePolicy, Exception e)
        {
            await Task.Factory.StartNew(() =>
            {
                var hash = proxiedMethodInvocation.GetHashString();
                var duration = TimeSpan.FromSeconds(cachePolicy.CacheDuration);
                var absoluteDuration = TimeSpan.FromSeconds(cachePolicy.MaxAge);

                _cache[hash] = new DictionaryCacheItem(e, duration, absoluteDuration);
            });
        }

        public async Task DeleteAsync<TResult>(Expression<Func<T, TResult>> expression)
        {
            await Task.Factory.StartNew(() =>
            {
                var invocation = ProxiedMethodInvocationGenerator<T>.FromExpression(expression);
                var hash = invocation.GetHashString<T, TResult>();
                _cache.Remove(hash);
            });
        }
    }
}
