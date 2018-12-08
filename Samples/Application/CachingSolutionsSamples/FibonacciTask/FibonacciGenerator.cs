using System.Collections.Generic;

namespace CachingSolutionsSamples.FibonacciTask
{
    /// <summary>
    /// Represents a <see cref="FibonacciGenerator"/> class.
    /// </summary>
    public static class FibonacciGenerator
    {
        /// <summary>
        /// Fibonacci generator.
        /// </summary>
        /// <param name="boarder">The boarder.</param>
        /// <returns>The <see cref="IEnumerable{int}"/></returns>
        public static IEnumerable<int> Generate(int boarder)
        {
            var firstNumber = 1;

            yield return firstNumber;

            if (boarder <= firstNumber)
                yield break;

            var secondNumber = 1;

            yield return secondNumber;

            if (boarder <= secondNumber)
                yield break;

            var summ = firstNumber + secondNumber;

            while (summ <= boarder)
            {
                checked
                {
                    summ = firstNumber + secondNumber;
                }

                if (summ <= boarder)
                {
                    firstNumber = secondNumber;

                    secondNumber = summ;

                    yield return summ;
                }
                else
                {
                    yield break;
                }
            }
        }
    }
}
