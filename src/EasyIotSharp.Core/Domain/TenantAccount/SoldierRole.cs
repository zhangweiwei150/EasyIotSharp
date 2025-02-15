using SqlSugar;

namespace EasyIotSharp.Core.Domain.TenantAccount
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    [SugarTable("SoldierRoles")]
    public class SoldierRole:BaseEntity<string>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantNumId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string SoldierId { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public string RoleId { get; set; }
    }
}
