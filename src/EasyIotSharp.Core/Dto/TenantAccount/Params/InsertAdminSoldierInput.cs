
namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 创建租户添加系统管理员的入参类
    /// </summary>
    public class InsertAdminSoldierInput
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 是否测试
        /// </summary>
        public bool IsTest { get; set; }

        /// <summary>
        /// 姓别：1=男，2=女，-1 = 不选
        /// </summary>
        public int Sex { get; set; } = -1;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
    }
}
