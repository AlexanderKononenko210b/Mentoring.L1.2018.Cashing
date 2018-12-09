using System;
using System.Collections.Generic;
using System.Linq;
using CachingSolutionsSamples.Interfaces;

namespace CachingSolutionsSamples.FibonacciTask
{
    /// <summary>
    /// Represents a <see cref="FibonacciService"/> class.
    /// </summary>
    public class FibonacciService
    {
        private readonly ICache<IEnumerable<int>> _cache;
        private const string FibonacciId = "Fibonacci";

        /// <summary>
        /// Initialize a <see cref="FibonacciService"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        public FibonacciService(ICache<IEnumerable<int>> cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Get Fibonacci numbers.
        /// </summary>
        /// <param name="boarder">The boarder.</param>
        /// <returns></returns>
        public IEnumerable<int> Start(int boarder)
        {
            if (boarder < 0)
                throw new ArgumentOutOfRangeException(nameof(boarder));

            var sequenceNumbers = _cache.Get(FibonacciId);
            var workList = new List<int>();

            if (sequenceNumbers == null || sequenceNumbers.ToList().Count <= 2)
            {
                Console.WriteLine("All numbers generate:");
                foreach (var number in FibonacciGenerator.Generate(boarder, 0, 1))
                {
                    workList.Add(number);
                }

                _cache.Set(FibonacciId, workList, DateTimeOffset.Now.AddSeconds(1));
                return workList;
            }

            if (sequenceNumbers.Last() < boarder)
            {
                Console.WriteLine("Combine cache and new generated numbers:");

                workList.AddRange(sequenceNumbers);
                var lastIndex = workList.Count - 1;
                foreach (var number in FibonacciGenerator.Generate(boarder, workList[lastIndex - 1], workList[lastIndex]))
                {
                    workList.Add(number);
                }

                _cache.Set(FibonacciId, workList, DateTimeOffset.Now.AddSeconds(1));
                return workList;
            }

            if (sequenceNumbers.Last() > boarder)
            {
                Console.WriteLine("Take part cache:");
                return sequenceNumbers.Where(number => number <= boarder);
            }

            Console.WriteLine("Take all cache:");
            return sequenceNumbers;
        }
    }
}
