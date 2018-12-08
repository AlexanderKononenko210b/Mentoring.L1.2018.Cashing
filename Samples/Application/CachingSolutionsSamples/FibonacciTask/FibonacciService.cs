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

            if (sequenceNumbers == null || sequenceNumbers.LastOrDefault() < boarder)
            {
                Console.WriteLine("Fibonacci generate:");
                var workList = new List<int>();
                foreach (var number in FibonacciGenerator.Generate(boarder))
                {
                    workList.Add(number);
                }

                _cache.Set(FibonacciId, workList, DateTimeOffset.Now.AddMinutes(10));
                return workList;
            }

            Console.WriteLine("Fibonacci numbers from cache:");
            return sequenceNumbers.Where(number => number <= boarder);
        }
    }
}
