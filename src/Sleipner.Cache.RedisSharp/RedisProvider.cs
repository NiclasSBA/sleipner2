using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sleipner.Cache.Netcore;
using Sleipner.Cache.Netcore.Model;
using Sleipner.Cache.Netcore.Policies;
using Sleipner.Cache.Netcore.RedisSharp.Wrapper;
using Sleipner.Core.Netcore.Util;

namespace Sleipner.Cache.Netcore.RedisSharp
{
    public class RedisProvider<T> : ICacheProvider<T> where T : class
    {

        private readonly IRedisClient _redis;
        private readonly string _hashScramble;
        private readonly JsonSerializer _serializer;

        public RedisProvider(IRedisClient client, string hashScramble = null)
        {
            _redis = client;
            _hashScramble = hashScramble;
            _serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }
        public Task DeleteAsync<TResult>(System.Linq.Expressions.Expression<Func<T, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<CachedObject<TResult>> GetAsync<TResult>(ProxiedMethodInvocation<T, TResult> proxiedMethodInvocation, CachePolicy cachePolicy)
        {
            var key = proxiedMethodInvocation.GetHashString(_hashScramble);
            var redisItem = await _redis.Get(key);

            if (redisItem != null)
            {
                RedisObject<TResult> cacheItem;
                using (var dataStream = new MemoryStream(redisItem))
                {

                    using (var zipstream = new GZipStream(dataStream, CompressionMode.Decompress))
                    {
                        using (var streamReader = new StreamReader(zipstream))
                        {
                            var textReader = new JsonTextReader(streamReader);

                            try
                            {
                                cacheItem = _serializer.Deserialize<RedisObject<TResult>>(textReader);
                            }
                            catch( JsonSerializationException)
                            {
                                return new CachedObject<TResult>(CachedObjectState.None, default(TResult));
                            }

                        }
                    }

                    if(cacheItem == null)
                         return new CachedObject<TResult>(CachedObjectState.None, default(TResult));
                         
                    if(cacheItem.IsException && cacheItem.Created.AddSeconds(cachePolicy.ExceptionCacheDuration) > DateTime.Now)
                    {
                        return new CachedObject<TResult>(CachedObjectState.Exception, cacheItem.Exception);
                    }else if (cacheItem.IsException)
                    {
                        
                        return new CachedObject<TResult>(CachedObjectState.None, default(TResult));
                    }

                            
                }
                
                
                var fresh = cacheItem.Created.AddSeconds(cachePolicy.CacheDuration) > DateTime.Now;
                var state = fresh ? CachedObjectState.Fresh : CachedObjectState.Stale;

                if (cachePolicy.DiscardStale && state == CachedObjectState.Stale)
                {
                    return new CachedObject<TResult>(CachedObjectState.None, default(TResult));
                }

                return new CachedObject<TResult>(state, cacheItem.Object);
            }

            return new CachedObject<TResult>(CachedObjectState.None, default(TResult));
        }


        public  async Task StoreAsync<TResult>(Core.Netcore.Util.ProxiedMethodInvocation<T, TResult> proxiedMethodInvocation, CachePolicy cachePolicy, TResult data)
        {
            var key = proxiedMethodInvocation.GetHashString(_hashScramble);
            var cachedObject = new RedisObject<TResult>()
             {
                 Created = DateTime.Now,
                 Object = data
             };

            var bytes = SerializeAndZip(cachedObject);

            if (cachePolicy.MaxAge > 0)
            {
                await _redis.Set(key, bytes, TimeSpan.FromSeconds(cachePolicy.MaxAge) );
            }
            else
            {
                await _redis.Set(key, bytes);
            }
        }

        public Task StoreExceptionAsync<TResult>(Core.Netcore.Util.ProxiedMethodInvocation<T, TResult> proxiedMethodInvocation, CachePolicy cachePolicy, Exception e)
        {
            throw new NotImplementedException();
        }

        private byte[] SerializeAndZip<TResult>(RedisObject<TResult> item)
        {
            using (var ms = new MemoryStream())
            {
                using (var compressionStream = new GZipStream(ms, CompressionMode.Compress))
                {
                    using (var zipStreamWriter = new StreamWriter(compressionStream))
                    {
                        _serializer.Serialize(zipStreamWriter, item);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
