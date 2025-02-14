
namespace EasyIotSharp.Core.Dto.Tenant.Params
{
    public class DeleteTenantInput : OperateUserInput
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
