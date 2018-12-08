using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.CacheModels;
using CachingSolutionsSamples.Managers;
using NorthwindLibrary;
using NUnit.Framework;

namespace Cache.Test
{
    [TestFixture]
    public class Task2Test
    {
        private const string CategoryPrefix = "Cache_Categories";
        private const string CustomerPrefix = "Cache_Customers";
        private const string OrderPrefix = "Cache_Orders";
        private const string HostName = "localhost,allowAdmin=true";
        private int _millisecondsOffset;
        private string _categoryQuery;
        private string _customerQuery;
        private string _orderQuery;


        [SetUp]
        public void Initialize()
        {
            int.TryParse(ConfigurationManager.AppSettings["offset"], out _millisecondsOffset);
            _categoryQuery = ConfigurationManager.AppSettings["categoryQuery"];
            _customerQuery = ConfigurationManager.AppSettings["customerQuery"];
            _orderQuery = ConfigurationManager.AppSettings["orderQuery"];
        }

        [Test]
        public void CategoryMemoryCacheWithDateTimeOffset()
        {
            var categoryManager = new MemoryCacheManager<Category>(
                new MemoryCache<IEnumerable<Category>>(CategoryPrefix));

            for (var i = 0; i < 10; i++)
            {
                var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow.AddMilliseconds(_millisecondsOffset));
                Console.WriteLine(categoryManager.GetData(dateTimeOffset).Count());
                Thread.Sleep(100);
            }
        }

        [Test]
        public void CustomerMemoryCacheWithDateTimeOffset()
        {
            var customerManager = new MemoryCacheManager<Customer>(
                new MemoryCache<IEnumerable<Customer>>(CustomerPrefix));

            for (var i = 0; i < 10; i++)
            {
                var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow.AddMilliseconds(_millisecondsOffset));
                Console.WriteLine(customerManager.GetData(dateTimeOffset).Count());
                Thread.Sleep(100);
            }
        }

        [Test]
        public void OrderMemoryCacheWithDateTimeOffset()
        {
            var orderManager = new MemoryCacheManager<Order>(
                new MemoryCache<IEnumerable<Order>>(OrderPrefix));

            for (var i = 0; i < 10; i++)
            {
                var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow.AddMilliseconds(_millisecondsOffset));
                Console.WriteLine(orderManager.GetData(dateTimeOffset).Count());
                Thread.Sleep(100);
            }
        }

        [Test]
        public void CategoryMemoryCacheWithCacheItemPolicy()
        {
            var categoryManager = new MemoryCacheManager<Category>(
                new MemoryCache<IEnumerable<Category>>(CategoryPrefix));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(categoryManager.GetData(_categoryQuery).Count());
                Thread.Sleep(100);
            }
        }

        [Test]
        public void CustomerMemoryCacheWithCacheItemPolicy()
        {
            var customerManager = new MemoryCacheManager<Customer>(
                new MemoryCache<IEnumerable<Customer>>(CustomerPrefix));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(customerManager.GetData(_customerQuery).Count());
                Thread.Sleep(100);
            }
        }

        [Test]
        public void OrderMemoryCacheWithCacheItemPolicy()
        {
            var orderManager = new MemoryCacheManager<Order>(
                new MemoryCache<IEnumerable<Order>>(OrderPrefix));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(orderManager.GetData(_orderQuery).Count());
                Thread.Sleep(100);
            }
        }

        [Test]
        public void CategoryRedisCacheWithDateTimeOffset()
        {
            var categoryManager = new RedisCacheManager<Category>(
                new RedisCache<IEnumerable<Category>>(HostName, CategoryPrefix));

            for (var i = 0; i < 10; i++)
            {
                var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow.AddMilliseconds(_millisecondsOffset));
                Console.WriteLine(categoryManager.GetData(dateTimeOffset).Count());
                Thread.Sleep(100);
            }
        }

        [Test]
        public void CustomerRedisCacheWithDateTimeOffset()
        {
            var customerManager = new RedisCacheManager<Customer>(
                new RedisCache<IEnumerable<Customer>>(HostName, CustomerPrefix));

            for (var i = 0; i < 10; i++)
            {
                var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow.AddMilliseconds(_millisecondsOffset));
                Console.WriteLine(customerManager.GetData(dateTimeOffset).Count());
                Thread.Sleep(100);
            }
        }

        [Test]
        public void OrderRedisCacheWithDateTimeOffset()
        {
            var orderManager = new RedisCacheManager<Order>(
                new RedisCache<IEnumerable<Order>>(HostName, OrderPrefix));

            for (var i = 0; i < 10; i++)
            {
                var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow.AddMilliseconds(_millisecondsOffset));
                Console.WriteLine(orderManager.GetData(dateTimeOffset).Count());
                Thread.Sleep(100);
            }
        }
    }
}
