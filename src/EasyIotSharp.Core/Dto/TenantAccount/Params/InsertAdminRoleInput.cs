
namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 创建租户添加一个租户管理员角色的入参类
    /// </summary>
    public class InsertAdminRoleInput
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
