using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 通过父级Url查询按钮路由的入参类
    /// </summary>
    public class QueryUrlMenuByParentUrlInput
    {
        /// <summary>
        /// 父级
        /// </summary>
        public string Url { get; set; }
    }
}
