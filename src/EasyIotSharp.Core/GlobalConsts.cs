using System.Collections.Generic;
using System.Linq;

namespace EasyIotSharp.Core
{
    /// <summary>
    /// 全局的常量
    /// </summary>
    public static class GlobalConsts
    {
        #region ES读写设置

        /// <summary>
        /// es 默认setting
        /// </summary>
        public static class ESSettings
        {
            /// <summary>
            /// es 评分系数
            /// </summary>
            public const double TIEBREAKER = 0.3;

            /// <summary>
            /// ES默认最大返回数量
            /// </summary>
            public const int MAXRESULTWINDOW = 90000;

            /// <summary>
            /// ES默认最大返回数量
            /// </summary>
            public const int DEFAULT = 10000;
        }

        #endregion ES读写设置

        #region 任务日志

        /// <summary>任务日志相关定义</summary>
        public static class TaskLog
        {
            /// <summary>
            /// 模块名称定义
            /// </summary>
            public static class Module
            {
                public const string POLYV_CHANNELCATEGORY = "直播分类";

                public const string POLYV_CHANNEL = "频道";

                public const string POLYV_WATCHLOG = "直播观看日志";

                public const string USER = "用户";
                public const string USER_SCORE = "用户成绩";
                public const string USER_COLLEGE = "用户院校";

                public const string LIVE_TOPIC = "直播主题";

                public const string WECHAT = "微信";

                public const string CHANNEL_SUBSCRIBE = "直播预约推送";
                public const string MINPROSUBSCRIBECONFIG = "小程序预约配置";

                public const string COLLEGE = "招办院校";

                public const string COLLEGEMENU = "招办栏目";

                public const string COLLEGECONTENTPAGE = "招办页面内容";

                public const string POLYVCHANNELREPORT = "直播报告";
            }

            /// <summary>任务日志模板定义</summary>
            public static class Template
            {
                public const string CREATE = "创建:{0}";
                public const string BULK_CREATE = "批量创建:{0}";
                public const string UPDATE = "修改:{0}";
                public const string BULK_UPDATE = "批量修改:{0}";
                public const string DELETE = "删除:{0}";
                public const string SYNC = "同步:{0}";
                public const string EXPOET = "导出:{0}";

                public const string INPUT = "入参:{0}";
                public const string EXEC = "执行:{0}";
                public const string ABORTED = "中止:{0}";

                public const string COMPLETED = "结束";

                public const string ELAPSED = "耗时:{0}s";
            }

            /// <summary>任务日志模板状态定义</summary>
            public static class Status
            {
                /// <summary>成功</summary>
                public const string SUCCESS = "成功";

                /// <summary>失败</summary>
                public const string FAIL = "失败";

                /// <summary> 状态值 </summary>
                public static string GetStatusValue(bool isSuccess)
                {
                    return isSuccess ? SUCCESS : FAIL;
                }
            }
        }

        #endregion 任务日志

        #region 缓存时间定义

        /// <summary>
        /// 缓存过期时间策略定义
        /// </summary>
        public static class CacheStrategy
        {
            /// <summary>
            /// 一天过期24小时
            /// </summary>

            public const int ONE_DAY = 1440;

            /// <summary>
            /// 12小时过期
            /// </summary>

            public const int HALF_DAY = 720;

            /// <summary>
            /// 8小时过期
            /// </summary>

            public const int EIGHT_HOURS = 480;

            /// <summary>
            /// 5小时过期
            /// </summary>

            public const int FIVE_HOURS = 300;

            /// <summary>
            /// 3小时过期
            /// </summary>

            public const int THREE_HOURS = 180;

            /// <summary>
            /// 2小时过期
            /// </summary>

            public const int TWO_HOURS = 120;

            /// <summary>
            /// 1小时过期
            /// </summary>

            public const int ONE_HOURS = 60;

            /// <summary>
            /// 半小时过期
            /// </summary>

            public const int HALF_HOURS = 30;

            /// <summary>
            /// 10分钟过期
            /// </summary>
            public const int TEN_MINUTES = 10;

            /// <summary>
            /// 永不过期
            /// </summary>

            public const int NEVER = -1;
        }

        #endregion 缓存时间定义

        #region 分页查询页大小定义

        /// <summary>分页查询页大小定义</summary>
        public static class PageSizeNums
        {
            /// <summary>页大小</summary>
            public const int _4 = 4;

            /// <summary>页大小</summary>
            public const int _5 = 5;

            /// <summary>每页10条</summary>
            public const int _10 = 10;

            /// <summary>每页20条</summary>
            public const int _20 = 20;

            /// <summary>每页50条</summary>
            public const int _50 = 50;

            /// <summary>每页100条</summary>
            public const int _100 = 100;

            /// <summary>每页1000条</summary>
            public const int _1000 = 1000;
        }

        #endregion 分页查询页大小定义


        /// <summary>
        /// 定义http请求方式
        /// </summary>
        public static class KnowHttpMethods
        {
            /// <summary>
            /// POST
            /// </summary>
            public const string POST = "POST";

            /// <summary>
            /// GET
            /// </summary>
            public const string GET = "GET";
        }

    }
}