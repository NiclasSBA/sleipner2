using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sleipner.Cache.Netcore.RedisSharp.Wrapper
{
    public class RedisClient : IRedisClient
    {

        private readonly ConnectionMultiplexer _multiplexer;
        public RedisClient(ConfigurationOptions options)
        {
            _multiplexer = ConnectionMultiplexer.Connect(options);
        }
        private  IDatabase RedisDatabase => _multiplexer.GetDatabase();
        public async Task<byte[]> Get(string key)
        {
            var value = (byte[])await RedisDatabase.StringGetAsync(key);
            return value;
        }

        public async Task Set(string key, byte[] value, TimeSpan? expiration)
        {
            await RedisDatabase.StringSetAsync(key, value, expiration);
        }

        public async Task Set(string key, byte[] value)
        {
            await RedisDatabase.StringSetAsync(key, value);
        }
    }
}
