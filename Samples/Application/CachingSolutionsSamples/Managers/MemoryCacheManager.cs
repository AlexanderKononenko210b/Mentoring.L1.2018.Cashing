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
    public class MemoryCacheManager<TModel, TResult>
        where TModel : class
        where TResult : class
    {
        private readonly IMemoryCache<TResult> _memoryCache;

        public MemoryCacheManager(IMemoryCache<TResult> memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public TResult GetData(DateTimeOffset dateTimeOffset)
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var items = _memoryCache.Get<TResult>(user);

            if (items == null)
            {
                Console.WriteLine("Data from Db:");

                using (var context = new Northwind())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;

                    var dBResult = context.Set<TModel>().ToList();

                    _memoryCache.Set(user, dBResult, dateTimeOffset);

                    return dBResult as TResult;
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
        public TResult GetData(string query)
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var items = _memoryCache.Get<TResult>(user);

            if (items == null)
            {
                Console.WriteLine("Data from Db:");

                using (var context = new Northwind())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    context.Configuration.LazyLoadingEnabled = false;

                    var dBResult = context.Set<TModel>().ToList();

                    _memoryCache.Set(user, dBResult, GetCacheItemPolicy(query));

                    return dBResult as TResult;
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
