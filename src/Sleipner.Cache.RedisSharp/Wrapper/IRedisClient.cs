using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sleipner.Cache.Netcore.RedisSharp.Wrapper
{
   public interface IRedisClient
    {
        Task<byte[]> Get(string key);
        Task Set(string key, byte[] value, TimeSpan? expiration);
        Task Set(string key, byte[] value);
    }
}
