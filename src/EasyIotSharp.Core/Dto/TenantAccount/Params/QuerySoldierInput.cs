using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    public class QuerySoldierInput:PagingInput
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 手机号/用户名
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 是否启用
        /// -1=不参与查询
        /// 1=启用
        /// 2=禁用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
