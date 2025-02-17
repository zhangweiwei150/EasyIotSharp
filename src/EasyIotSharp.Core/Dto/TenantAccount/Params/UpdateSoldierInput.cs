
namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 通过id修改一条用户信息的入参类
    /// </summary>
    public class UpdateSoldierInput
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 姓别：1=男，2=女，-1 = 不选
        /// </summary>
        public int Sex { get; set; } = -1;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否修改角色
        /// </summary>
        public bool IsUpdateRole { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public string RoleId { get; set; }
    }
}
