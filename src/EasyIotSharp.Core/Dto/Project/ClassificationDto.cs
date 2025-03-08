using EasyIotSharp.Core.Domain.Proejct;
using System;
using System.Collections.Generic;
using System.Text;
using UPrime.AutoMapper;

namespace EasyIotSharp.Core.Dto.Project
{
    /// <summary>
    /// 代表一条项目分类的“DTO”
    /// </summary>
    [AutoMapFrom(typeof(Classification))]
    public class ClassificationDto
    {
        /// <summary>
        /// 项目分类Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantNumId { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序字段
        /// 字段越大越靠前
        /// </summary>
        public int Sort { get; set; }

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
