using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 通过id删除一条用户信息的入参类
    /// </summary>
    public class DeleteSoldierInput
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否删除 
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
