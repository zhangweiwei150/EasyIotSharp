using EasyIotSharp.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using EasyIotSharp.Core.Repositories.Mysql;
using System.Threading.Tasks;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;

namespace EasyIotSharp.Core.Repositories.Tenant.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class TenantRepository : MySqlRepositoryBase<EasyIotSharp.Core.Domain.Tenant.Tenant,string>, ITenantRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseProvider"></param>
        public TenantRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {

        }

        public async Task<(int toalCount, List<EasyIotSharp.Core.Domain.Tenant.Tenant> items)> Query(string keyword, 
                                                                                                     int expiredType, 
                                                                                                     DateTime? contractStartTime, 
                                                                                                     DateTime? contractEndTime, 
                                                                                                     int isFreeze, 
                                                                                                     int pageIndex, 
                                                                                                     int pageSize)
        {
            var sql = "SELECT * FROM Tenants where 1=1 and IsDelete=false ";
            string pageStr = $"LIMIT {pageSize} OFFSET ({pageIndex} - 1) * {pageSize}";
            string whereStr = default;
            if (!string.IsNullOrWhiteSpace(keyword)) 
            {
                whereStr += $" and Name like '%{keyword}%'";
            }
            if (expiredType>-1)
            {
                switch (expiredType)
                {
                    //待授权
                    case 0:

                        whereStr += $" and ContractStartTime>{DateTime.Now}";

                        break;

                    //生效中
                    case 1:

                        whereStr += $" and ContractStartTime<{DateTime.Now} and ContractEndTime>{DateTime.Now}";

                        break;

                    //已过期
                    case 2:

                        whereStr += $" and ContractEndTime<{DateTime.Now}";

                        break;

                    default:
                        break;
                }
            }
            if (contractStartTime.IsNotNull()&&contractEndTime.IsNotNull())
            {
                whereStr += $" and ContractStartTime<{DateTime.Now} and ContractEndTime>{DateTime.Now}";
            }
            else
            {
                if (contractStartTime.IsNotNull())
                {
                    whereStr += $" and ContractStartTime<{DateTime.Now}";
                }
                if (contractEndTime.IsNotNull())
                {
                    whereStr += $" and ContractEndTime>{DateTime.Now}";
                }
            }
            if (isFreeze > -1)
            {
                whereStr += $" and IsFreeze={(isFreeze == 1 ? true : false)}";
            }
            string totalCountSql = "SELECT count(1) FROM Tenants where 1=1 and IsDelete=false " + whereStr + pageStr;
            var totalCount = await Client.Ado.GetIntAsync(totalCountSql);
            if (totalCount<=0)
            {
                return (0, new List<Domain.Tenant.Tenant>());
            }
            string sortStr = " ORDER BY CreationTime DESC";
            var items = await Client.Ado.SqlQueryAsync<EasyIotSharp.Core.Domain.Tenant.Tenant>(sql + whereStr + sortStr + pageStr);
            return (totalCount,items);
        }
    }
}
