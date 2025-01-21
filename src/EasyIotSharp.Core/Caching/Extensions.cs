using System;
using System.Threading.Tasks;
using UPrime.Runtime.Caching;
using static EasyIotSharp.Core.GlobalConsts;

namespace EasyIotSharp.Core.Caching
{
    /// <summary>
    /// 缓存扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 加密Key的参数，主要为了加密中文等非法字符（统一用一种加密方式） 如果感觉参数值有可能会出现非法，则使用此方法加密
        /// </summary>
        /// <param name="keyParam"></param>
        /// <returns></returns>
        public static string EncryptKeyParam(this string keyParam)
        {
            if (keyParam.IsNotNullOrEmpty())
                return keyParam.EncodeUTF8Base64();
            else
                return "";
        }

        /// <summary>
        /// 获取缓存扩展
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <param name="strategy">指定缓存策略</param>
        /// <returns></returns>
        public static async Task<TValue> GetAsyncExt<TKey, TValue>(this ICache cache, TKey key, Func<Task<TValue>> factory, TimeSpan strategy)
        {
            if (strategy == TimeSpan.Zero)
                return await cache.GetAsync(key, factory);
            else
                return await cache.GetAsync(key, factory, strategy);
        }

        /// <summary>
        /// 获取缓存扩展
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <param name="strategy">指定缓存策略</param>
        /// <returns></returns>
        public static TValue GetExt<TKey, TValue>(this ICache cache, TKey key, Func<TValue> factory, int strategy)
        {
            if (strategy == CacheStrategy.NEVER)
                return cache.Get(key, factory);
            else
                return cache.Get(key, factory, TimeSpan.FromMinutes(strategy));
        }

        /// <summary>按天缓存时间</summary>
        public static TimeSpan Days(this int time) => TimeSpan.FromDays(time);

        /// <summary>按小时缓存时间</summary>
        public static TimeSpan Hours(this int time) => TimeSpan.FromHours(time);

        /// <summary>按分钟缓存时间</summary>
        public static TimeSpan Minutes(this int time) => TimeSpan.FromMinutes(time);

        /// <summary>按秒缓存时间</summary>
        public static TimeSpan Seconds(this int time) => TimeSpan.FromSeconds(time);
    }
}