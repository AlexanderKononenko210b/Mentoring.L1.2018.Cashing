using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Caching;
using System.Threading;
using CachingSolutionsSamples.Interfaces;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Managers
{
    public class MemoryCacheManager<T>
        where T : class
    {
        private readonly IMemoryCache<T> _memoryCache;

        public MemoryCacheManager(IMemoryCache<T> memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T GetData(DateTimeOffset dateTimeOffset)
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var items = _memoryCache.Get<T>(user);

            if (items == null)
            {
                Console.WriteLine("Data from Db:");

                using (var context = new Northwind())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;

                    var dbResult = DbDownloader<T>.GetResultFromDb(context);

                    _memoryCache.Set(user, dbResult, dateTimeOffset);

                    return dbResult;
                }
            }

            Console.WriteLine("Data from cache:");

            return items;
        }

        /// <summary>
        /// Get data.
        /// </summary>
        /// <param name="query">The query for create SqlDependency.</param>
        /// <returns>The  <see cref="IEnumerable{T}"/></returns>
        public T GetData(string query)
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var items = _memoryCache.Get<T>(user);

            if (items == null)
            {
                Console.WriteLine("Data from Db:");

                using (var context = new Northwind())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;

                    var dbResult = DbDownloader<T>.GetResultFromDb(context);

                    _memoryCache.Set(user, dbResult, GetCacheItemPolicy(query));

                    return dbResult;
                }
            }

            Console.WriteLine("Data from cache:");
            return items;
        }

        /// <summary>
        /// Get cache item policy.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The <see cref="CacheItemPolicy"/> instance.</returns>
        private CacheItemPolicy GetCacheItemPolicy(string query)
        {
            var policy = new CacheItemPolicy();
            policy.ChangeMonitors.Add(
                new SqlChangeMonitor(
                    new SqlDependency(
                        new SqlCommand(query))));

            return policy;
        }
    }
}
