using System;
using SqlSugar;
using System.Reflection;
using UPrime.Domain.Entities.Auditing;
using UPrime.Domain.Entities;

namespace EasyIotSharp.Core.Domain
{
    /// <summary>
    /// 实体类基类
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class BaseEntity<TPrimaryKey>: IEntity<TPrimaryKey>
    {
        public BaseEntity()
        {
            CreationTime = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public TPrimaryKey Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? DeleteTime { get; set; }

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

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public bool IsTransient()
        {
            return Id.IsNotNull();
        }
    }
}