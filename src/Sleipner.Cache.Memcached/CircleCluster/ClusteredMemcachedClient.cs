﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemcachedSharp;

namespace Sleipner.Cache.Memcached.CircleCluster
{
    public class ClusteredMemcachedClient : IMemcachedClient
    {
        public readonly string EndPoint;
        private readonly MemcachedClient _client;
        public bool IsAlive;

        public ClusteredMemcachedClient(string endPoint, MemcachedOptions options = null)
        {
            EndPoint = endPoint;
            _client = new MemcachedClient(endPoint, options);
            IsAlive = true;
        }

        protected bool Equals(ClusteredMemcachedClient other)
        {
            return string.Equals(EndPoint, other.EndPoint);
        }

        public override int GetHashCode()
        {
            return (EndPoint != null ? EndPoint.GetHashCode() : 0);
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ClusteredMemcachedClient) obj);
        }

        public Task<MemcachedItem> Get(string key)
        {
            return _client.Get(key);
        }

        public Task<MemcachedItem> Gets(string key)
        {
            return _client.Gets(key);
        }

        public Task Set(string key, byte[] value, MemcachedStorageOptions options = null)
        {
            return _client.Set(key, value, options);
        }

        public Task<bool> Delete(string key)
        {
            return _client.Delete(key);
        }

        public Task<bool> Add(string key, byte[] value, MemcachedStorageOptions options = null)
        {
            return _client.Add(key, value, options);
        }

        public Task<bool> Replace(string key, byte[] value, MemcachedStorageOptions options = null)
        {
            return _client.Replace(key, value, options);
        }

        public Task<bool> Append(string key, byte[] value, MemcachedStorageOptions options = null)
        {
            return _client.Add(key, value, options);
        }

        public Task<bool> Prepend(string key, byte[] value, MemcachedStorageOptions options = null)
        {
            return _client.Prepend(key, value, options);
        }

        public Task<ulong?> Increment(string key, ulong value)
        {
            return _client.Increment(key, value);
        }

        public Task<ulong?> Decrement(string key, ulong value)
        {
            return _client.Decrement(key, value);
        }

        public Task<CasResult> Cas(string key, long casUnique, byte[] value, MemcachedStorageOptions options = null)
        {
            return _client.Cas(key, casUnique, value, options);
        }
    }
}
