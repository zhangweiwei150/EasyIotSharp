using EasyIotSharp.Core.Dto.Tenant;
using EasyIotSharp.Core.Dto.Users;

namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 用户验证登录的出参类
    /// </summary>
    public class ValidateSoldierOutput
    {
        /// <summary>
        /// 验证状态
        /// 1=成功
        /// 2=用户名或密码有误
        /// 3=账号已禁用
        /// 21=租户不存在
        /// 22=租户已被删除
        /// 23=租户已被冻结
        /// 24=租户已过期
        /// 51=登录失败次数过多，账号锁定30分钟
        /// </summary>
        public ValidateSoldierStatus Status { get; set; } = ValidateSoldierStatus.Success;

        /// <summary>
        /// 验证成功后返回的Token信息（包括过期时间）
        /// </summary>
        public UserTokenDto Token { get; set; }

        /// <summary>
        /// 获取的用户信息
        /// </summary>
        public SoldierDto Solider { get; set; }

        /// <summary>
        /// 获取的租户信息
        /// </summary>
        public TenantDto Tenant { get; set; }
    }
    /// <summary>
    /// 验证状态
    /// 1=成功
    /// 2=用户名或密码有误
    /// 3=账号已禁用
    /// 21=租户不存在
    /// 22=租户已被删除
    /// 23=租户已被冻结
    /// 24=租户已过期
    /// 51=登录失败次数过多，账号锁定30分钟
    /// </summary>
    public enum ValidateSoldierStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 用户名或密码有误
        /// </summary>
        InvalidNameOrPassword = 2,

        /// <summary>
        /// 账号已禁用
        /// </summary>
        SoldiersIsDisable = 3,

        /// <summary>
        /// 租户不存在
        /// </summary>
        TenantIsNotExists = 21,

        /// <summary>
        /// 租户已被删除
        /// </summary>
        TenantIsDeleted = 22,

        /// <summary>
        /// 租户已被冻结
        /// </summary>
        TenantIsFreeze = 23,

        /// <summary>
        /// 租户已过期
        /// </summary>
        TenantIsExpired = 24,

        /// <summary>
        /// 登录失败次数过多，账号锁定30分钟
        /// </summary>
        SoldiersLock = 51,
    }
}
