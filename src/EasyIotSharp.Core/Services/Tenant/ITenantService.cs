using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyIotSharp.Core.Dto.Tenant.Params;

namespace EasyIotSharp.Core.Services.Tenant
{
    public interface ITenantService
    {
        /// <summary>
        /// 添加一个租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task InsertTenant(InsertTenantInput input);
    }
}
