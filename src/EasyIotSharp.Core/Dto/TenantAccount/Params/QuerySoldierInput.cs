
namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 通过条件分页查询用户列表的入参类
    /// </summary>
    public class QuerySoldierInput:PagingInput
    {

        /// <summary>
        /// 手机号/用户名
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 是否启用
        /// -1=不参与查询
        /// 1=启用
        /// 2=禁用
        /// </summary>
        public int IsEnable { get; set; }
    }
}
