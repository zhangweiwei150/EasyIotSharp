namespace EasyIotSharp.Core.Caching
{
    public class CachingConsts
    {
        /// <summary>
        /// 缓存的所有KEY的定义
        /// </summary>
        public static class Keys
        {
            /// <summary>
            /// 项目缓存前缀（清除所有用）
            /// </summary>
            public const string BASE = "service:EasyIotSharp";
        }

        /// <summary>
        /// 缓存默认过期 = 12小时
        /// </summary>
        public const int DEFAULT_EXPIRES_MINUTES = 720;

        /// <summary>
        /// 检索相关的缓存过期时间（相对精准）
        /// </summary>
        public const int QUERY_EXPIRES_MINUTES = 120;

        /// <summary>
        /// 检索相关的缓存过期时间（非精准关键字） = 1小时
        /// </summary>
        public const int SEARCH_EXPIRES_MINUTES = 60;

        /// <summary>
        /// 用户相关的缓存过期时间 = 2小时
        /// </summary>
        public const int USER_EXPIRES_MINUTES = 120;

        /// <summary>
        /// 会员卡相关的缓存过期时间= 1小时
        /// </summary>
        public const int CARD_EXPIRES_MINUTES = 60;

        /// <summary>
        /// 测评相关的缓存过期时间= 1小时
        /// </summary>
        public const int EVALUATION_EXPIRES_MINUTES = 60;

        /// <summary>
        /// 单个专业定位测评详情相关的缓存过期时间= 半小时
        /// </summary>
        public const int PROFESSIONORIENTATION_EXPIRES_MINUTES = 30;

        /// <summary>
        /// 缓存10分钟过期
        /// </summary>
        public const int TEN_EXPIRES_MINUTES = 10;

        /// <summary>
        /// 缓存30分钟过期
        /// </summary>
        public const int THIRTY_EXPIRES_MINUTES = 30;

        /// <summary>
        /// 缓存24小时过期
        /// </summary>
        public const int TWENTY_FOUR_EXPIRES_HOURS = 24;

        /// <summary>
        /// 微信开发平台授权ACCESSTOKEN过期时间（官方默认过期时间7200秒，考虑接口调用耗时，本地缓存缩短30秒）
        /// </summary>
        public const int ACCESSTOKEN_EXPIRED_SECONDS = 700;
    }
}