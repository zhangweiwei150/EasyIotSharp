using System;

namespace EasyIotSharp.Core.Dto.Tenant.Params
{
    /// <summary>
    /// 通过条件分页查询租户列表的入参类
    /// </summary>
    public class QueryTenantInput : PagingInput
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 过期状态
        /// -1=不参与查询
        /// 0=待授权
        /// 1=生效
        /// 2=已过期
        /// </summary>
        public int ExpiredType { get; set; }

        /// <summary>
        /// 合同开始时间
        /// </summary>
        public DateTime? ContractStartTime { get; set; }

        /// <summary>
        /// 合同结束时间
        /// </summary>
        public DateTime? ContractEndTime { get; set; }

        /// <summary>
        /// 冻结状态
        /// -1=不参与查询
        /// 1=已冻结
        /// 2=未冻结
        /// </summary>
        public int IsFreeze { get; set; }
    }
}
