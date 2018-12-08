using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using CachingSolutionsSamples.Interfaces;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Managers
{
    /// <summary>
    /// Represents a <see cref="MemoryCacheManager{T}"/> class.
    /// </summary>
    public class MemoryCacheManager<T>
        where T : class
    {
        private readonly IMemoryCache<IEnumerable<T>> _memoryCache;

        /// <summary>
        /// Initialize a <see cref="RedisCacheManager{T}"/> instance.
        /// </summary>
        /// <param name="memoryCache">The memory cache.</param>
        public MemoryCacheManager(IMemoryCache<IEnumerable<T>> memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Get data.
        /// </summary>
        /// <param name="dateTimeOffset">The dateTimeOffset.</param>
        /// <returns>The  <see cref="IEnumerable{T}"/></returns>
        public IEnumerable<T> GetData(DateTimeOffset dateTimeOffset)
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var items = _memoryCache.Get(user);

            if (items == null)
            {
                Console.WriteLine("Data from Db:");

                using (var context = new Northwind())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;
                    items = context.Set<T>().ToList();
                    _memoryCache.Set(user, items, dateTimeOffset);
                }
            }
            else
            {
                Console.WriteLine("Data from cache:");
            }

            return items;
        }

        /// <summary>
        /// Get data.
        /// </summary>
        /// <param name="query">The query for create SqlDependency.</param>
        /// <returns>The  <see cref="IEnumerable{T}"/></returns>
        public IEnumerable<T> GetData(string query)
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var items = _memoryCache.Get(user);

            if (items == null)
            {
                Console.WriteLine("Data from Db:");

                using (var context = new Northwind())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;
                    items = context.Set<T>().ToList();
                    
                    _memoryCache.Set(user, items, GetCacheItemPolicy(query));
                }
            }
            else
            {
                Console.WriteLine("Data from cache:");
            }

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
