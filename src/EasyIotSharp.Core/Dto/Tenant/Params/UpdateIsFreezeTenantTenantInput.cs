
namespace EasyIotSharp.Core.Dto.Tenant.Params
{
    /// <summary>
    /// 通过id修改一个租户的冻结状态得入参类
    /// </summary>
    public class UpdateIsFreezeTenantTenantInput : OperateUserInput
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 冻结描述
        /// </summary>
        public string FreezeDes { get; set; }
    }
}
