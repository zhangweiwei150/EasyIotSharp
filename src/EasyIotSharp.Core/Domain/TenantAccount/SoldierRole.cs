using SqlSugar;

namespace EasyIotSharp.Core.Domain.TenantAccount
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    [SugarTable("SoldierRoles")]
    public class SoldierRole:BaseEntity<int>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int SoldierId { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public int RoleId { get; set; }
    }
}
