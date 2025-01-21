using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Tenant.Params
{
    public class InsertTenantInput:OperateUserInput
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        public string Name { get; set; }
    }
}
