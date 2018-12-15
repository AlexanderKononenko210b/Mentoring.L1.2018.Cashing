using System;
using System.Runtime.Caching;
using CachingSolutionsSamples.Interfaces;

namespace CachingSolutionsSamples.CacheModels
{
    /// <summary>
    /// Represents a <see cref="MemoryCache"/> class.
    /// </summary>
    public class MemoryCache<T> : IMemoryCache<T>
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        private readonly string _prefix;

        public MemoryCache(string prefix)
        {
            _prefix = prefix;
        }
        
        ///<inheritdoc/>
        public T Get<T>(string user)
        {
            var key = $"{_prefix}{user}";
            return (T) _cache.Get(key);
        }

        ///<inheritdoc/>
        public void Set<T>(string user, T data, DateTimeOffset dateTimeOffset)
        {
            var key = $"{_prefix}{user}";
            _cache.Set(key, data, dateTimeOffset);
        }

        ///<inheritdoc/>
        public void Set<T>(string user, T data, CacheItemPolicy policy)
        {
            var key = $"{_prefix}{user}";
            _cache.Set(key, data, policy);
        }
    }
}
