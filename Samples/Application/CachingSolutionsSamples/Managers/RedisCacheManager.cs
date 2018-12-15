using System;
using System.Collections.Generic;
using System.Threading;
using CachingSolutionsSamples.Interfaces;
using NorthwindLibrary;


namespace CachingSolutionsSamples.Managers
{
    /// <summary>
    /// Represents a <see cref="RedisCacheManager{T}"/> class.
    /// </summary>
    public class RedisCacheManager<T>
        where T : class
    {
        private readonly ICache<T> _redisCache;

        /// <summary>
        /// Initialize a <see cref="RedisCacheManager{T}"/> instance.
        /// </summary>
        /// <param name="redisCache"></param>
        public RedisCacheManager(ICache<T> redisCache)
        {
            _redisCache = redisCache;
        }

        /// <summary>
        /// Get data.
        /// </summary>
        /// <param name="dateTimeOffset">The dateTimeOffset.</param>
        /// <returns>The  <see cref="IEnumerable{T}"/></returns>
        public T GetData(DateTimeOffset dateTimeOffset)
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var orders = _redisCache.Get<T>(user);

            if (orders == null)
            {
                Console.WriteLine("Data from Db:");

                using (var context = new Northwind())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;
                    var dbResult = DbDownloader<T>.GetResultFromDb(context);
                    _redisCache.Set(user, dbResult, dateTimeOffset);

                    return dbResult;
                }
            }

            Console.WriteLine("Data from cache:");
            return orders;
        }
    }
}
