using System;

namespace EasyIotSharp.Core.Dto.Queue
{
    /// <summary>
    /// 代表一条RabbitMQ服务器配置信息的"DTO"
    /// </summary>
    public class RabbitServerInfoDto
    {
        public string id { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        public int TenantNumId { get; set; }

        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}