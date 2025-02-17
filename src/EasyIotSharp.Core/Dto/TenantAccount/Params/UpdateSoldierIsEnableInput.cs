using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 根据id修改用户的是否启用状态的入参类
    /// </summary>
    public class UpdateSoldierIsEnableInput
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
