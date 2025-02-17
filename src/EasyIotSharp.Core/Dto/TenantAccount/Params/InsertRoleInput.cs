
using System.Collections.Generic;

namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 添加一条角色信息的入参类
    /// </summary>
    public class InsertRoleInput
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        
        /// <summary>
        /// 子集菜单id集合
        /// </summary>
        public List<string> Menus { get; set; }
    }
}
