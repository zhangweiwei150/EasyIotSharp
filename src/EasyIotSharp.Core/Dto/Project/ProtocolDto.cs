using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project
{
    /// <summary>
    /// 代表一条协议信息的"DTO"
    /// </summary>
    public class ProtocolDto
    {
        /// <summary>
        /// 协议id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 协议名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 协议描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 操作人标识
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }
    }
}
