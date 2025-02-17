
namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 通过id修改一条角色信息的是否启用状态的入参类
    /// </summary>
    public class UpdateIsEnableRoleInput
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
