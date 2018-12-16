using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Interfaces;
using NorthwindLibrary;


namespace CachingSolutionsSamples.Managers
{
    /// <summary>
    /// Represents a <see cref="RedisCacheManager{TModel, TResult}"/> class.
    /// </summary>
    public class RedisCacheManager<TModel, TResult>
        where TModel : class
        where TResult : class
    {
        private readonly ICache<TResult> _redisCache;

        /// <summary>
        /// Initialize a <see cref="RedisCacheManager{TModel, TResult}"/> instance.
        /// </summary>
        /// <param name="redisCache"></param>
        public RedisCacheManager(ICache<TResult> redisCache)
        {
            _redisCache = redisCache;
        }

        /// <summary>
        /// Get data.
        /// </summary>
        /// <param name="dateTimeOffset">The dateTimeOffset.</param>
        /// <returns>The  <see cref="IEnumerable{T}"/></returns>
        public TResult GetData(DateTimeOffset dateTimeOffset)
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var orders = _redisCache.Get<TResult>(user);

            if (orders == null)
            {
                Console.WriteLine("Data from Db:");

                using (var context = new Northwind())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;
                    var dbResult = context.Set<TModel>().ToList();
                    _redisCache.Set(user, dbResult, dateTimeOffset);

                    return dbResult as TResult;
                }
            }

            Console.WriteLine("Data from cache:");
            return orders;
        }
    }
}
