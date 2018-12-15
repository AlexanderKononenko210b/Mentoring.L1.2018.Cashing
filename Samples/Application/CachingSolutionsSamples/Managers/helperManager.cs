using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Managers
{
    public static class DbDownloader<T>
        where T : class
    {
        /// <summary>
        /// Get result from db.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The <see cref="T"/></returns>
        public static T GetResultFromDb(Northwind context)
        {
            T dbResult;
            if (typeof(T).IsGenericType)
            {
                var argumentType = typeof(T).GetGenericArguments()[0];
                var resultFromDb = context.Set(argumentType).ToListAsync().Result;

                var entityGenericListType = typeof(List<>).MakeGenericType(argumentType);
                var entityGenericList = Activator.CreateInstance(entityGenericListType);
                var addMethodInfo = entityGenericListType.GetMethod("Add");

                foreach (var item in resultFromDb)
                {
                    addMethodInfo?.Invoke(entityGenericList, new[] { Convert.ChangeType(item, argumentType) });
                }

                dbResult = (T)entityGenericList;

            }
            else
            {
                dbResult = context.Set<T>().ToList() as T;
            }

            return dbResult;
        }
    }
}
