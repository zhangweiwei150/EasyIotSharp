using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.Proejct
{
    /// <summary>
    /// 分类表
    /// </summary>
    /// <remarks>项目下面的分类，一个类别里面关联的是多个测点</remarks>
    public class Classification:BaseEntity<string>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantNumId { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序字段
        /// 字段越大越靠前
        /// </summary>
        public int Sort { get; set; }
    }
}
