using System;
using System.Collections.Generic;

namespace EasyIotSharp.Core.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 根据集合中指定的列过滤重复
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T, TResult>(this IEnumerable<T> source, Func<T, TResult> where)
        {
            HashSet<TResult> hashSetData = new HashSet<TResult>();
            foreach (T item in source)
            {
                if (hashSetData.Add(where(item)))
                {
                    yield return item;
                }
            }
        }
    }
}