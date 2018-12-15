using System;
using System.IO;
using System.Runtime.Caching;
using System.Runtime.Serialization;
using CachingSolutionsSamples.Interfaces;
using StackExchange.Redis;

namespace CachingSolutionsSamples.CacheModels
{
    /// <summary>
    /// Represents a <see cref="RedisCache{T}"/> class.
    /// </summary>
    public class RedisCache<T> : ICache<T>
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly string _prefix;
        private readonly DataContractSerializer _serializer = new DataContractSerializer(typeof(T));

        /// <summary>
        /// Initialize a <see cref="RedisCache{T}"/> instance.
        /// </summary>
        /// <param name="hostName">The host name.</param>
        /// <param name="prefix">The prefix.</param>
        public RedisCache(string hostName, string prefix)
        {
            _redisConnection = ConnectionMultiplexer.Connect(hostName);
            _prefix = prefix;
        }

        ///<inheritdoc/>
        public T Get<T>(string user)
        {
            var database = _redisConnection.GetDatabase();

            var key = $"{_prefix}{user}";

            if (!database.KeyExists(key)) return default(T);

            byte[] orders = database.StringGet(key);

            if (orders == null) return default(T);

            return (T)_serializer.ReadObject(new MemoryStream(orders));
        }

        ///<inheritdoc/>
        public void Set<T>(string user, T data, DateTimeOffset dateTimeOffset)
        {
            var database = _redisConnection.GetDatabase();

            var key = $"{_prefix}{user}";

            if (data == null)
            {
                database.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                _serializer.WriteObject(stream, data);
                database.StringSet(key, stream.ToArray());
                database.KeyExpire(key, dateTimeOffset - DateTimeOffset.Now);
            }
        }
    }
}
