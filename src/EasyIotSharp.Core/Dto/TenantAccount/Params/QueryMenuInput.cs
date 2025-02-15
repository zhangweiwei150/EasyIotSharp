using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    public class QueryMenuInput:PagingInput
    {
        /// <summary>
        /// 菜单名称
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
