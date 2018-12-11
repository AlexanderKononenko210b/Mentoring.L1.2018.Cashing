using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.CacheModels;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Managers
{
    /// <summary>
    /// Represents a <see cref="RedisCacheManager{T}"/> class.
    /// </summary>
    public class RedisCacheManager<T>
        where T : class
    {
        private readonly RedisCache<IEnumerable<T>> _redisCache;

        /// <summary>
        /// Initialize a <see cref="RedisCacheManager{T}"/> instance.
        /// </summary>
        /// <param name="redisCache"></param>
        public RedisCacheManager(RedisCache<IEnumerable<T>> redisCache)
        {
            _redisCache = redisCache;
        }

        /// <summary>
        /// Get data.
        /// </summary>
        /// <param name="dateTimeOffset">The dateTimeOffset.</param>
        /// <returns>The  <see cref="IEnumerable{T}"/></returns>
        public IEnumerable<T> GetData(DateTimeOffset dateTimeOffset)
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var orders = _redisCache.Get(user);

            if (orders == null)
            {
                Console.WriteLine("Data from Db:");

                using (var context = new Northwind())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;
                    orders = context.Set<T>().ToList();
                    _redisCache.Set(user, orders, dateTimeOffset);
                }
            }
            else
            {
                Console.WriteLine("Data from cache:");
            }

            return orders;
        }
    }
}
