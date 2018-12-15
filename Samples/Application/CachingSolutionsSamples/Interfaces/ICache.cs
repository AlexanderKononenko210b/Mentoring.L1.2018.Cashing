using System;
using System.Runtime.Caching;

namespace CachingSolutionsSamples.Interfaces
{
    /// <summary>
    /// Represents an <see cref="ICache{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICache<T>
	{
        /// <summary>
        /// Get data type <see cref="T"/> from cache.
        /// </summary>
        /// <param name="user">The user identification.</param>
        /// <returns>The <see cref="T"/></returns>
		T Get<T>(string user);

        /// <summary>
        /// Set data type <see cref="T"/> to cache.
        /// </summary>
        /// <param name="user">The user identification.</param>
        /// <param name="data">The date.</param>
        /// <param name="dateTimeOffset">The date time offset.</param>
		void Set<T>(string user, T data, DateTimeOffset dateTimeOffset);
    }
}
