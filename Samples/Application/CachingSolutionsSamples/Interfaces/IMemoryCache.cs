using System.Runtime.Caching;

namespace CachingSolutionsSamples.Interfaces
{
    /// <summary>
    /// Represents an <see cref="IMemoryCache{T}"/> interface.
    /// </summary>
    public interface IMemoryCache<T> : ICache<T>
    {
        /// <summary>
        /// Set data type <see cref="T"/> to cache.
        /// </summary>
        /// <param name="user">The user identification.</param>
        /// <param name="data">The date.</param>
        /// <param name="cacheItemPolicy">The cache item policy.</param>
        void Set(string user, T data, CacheItemPolicy cacheItemPolicy);
    }
}
