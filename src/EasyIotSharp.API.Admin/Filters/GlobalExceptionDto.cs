namespace EasyIotSharp.API.Admin.Filters
{
    public class GlobalExceptionDto
    {
        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// 查询参数
        /// </summary>
        public string QueryString { get; set; }

        /// <summary>
        /// 请求体
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 异常消息
        /// </summary>
        public string Exception { get; set; }
    }
}