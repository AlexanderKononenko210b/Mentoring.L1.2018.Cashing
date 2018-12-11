using System;
using System.Collections.Generic;
using CachingSolutionsSamples.CacheModels;
using CachingSolutionsSamples.FibonacciTask;
using NUnit.Framework;

namespace Cache.Test
{
    /// <summary>
    /// Summary description for Task1Test
    /// </summary>
    [TestFixture]
    public class Task1Test
    {
        private const string HostName = "localhost,allowAdmin=true";
        private const string RedisPrefix = "Redis";
        private const string MemoryPrefix = "Memory";

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(15)]
        [TestCase(2315)]
        [TestCase(223415)]
        [TestCase(35)]
        [TestCase(115)]
        [TestCase(223415)]
        public void FibonacciRedis(int boarder)
        {
            var fibonacciService = new FibonacciService(new RedisCache<IEnumerable<int>>(HostName, RedisPrefix));

            foreach (var number in fibonacciService.Start(boarder))
            {
                Console.Write($"{number} ");
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(15)]
        [TestCase(2315)]
        [TestCase(223415)]
        [TestCase(35)]
        [TestCase(115)]
        [TestCase(223415)]
        public void FibonacciMemory(int boarder)
        {
            var fibonacciService = new FibonacciService(new MemoryCache<IEnumerable<int>>(MemoryPrefix));

            foreach (var number in fibonacciService.Start(boarder))
            {
                Console.Write($"{number} ");
            }
        }
    }
}
