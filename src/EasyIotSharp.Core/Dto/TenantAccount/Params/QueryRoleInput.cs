﻿
namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 通过条件查询角色列表的入参类
    /// </summary>
    public class QueryRoleInput:PagingInput
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantNumId { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 是否启用
        /// -1=不参与查询
        /// 1=启用
        /// 2=禁用
        /// </summary>
        public int IsEnable { get; set; }
    }
}
