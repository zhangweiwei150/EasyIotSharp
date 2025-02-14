using EasyIotSharp.Core.Dto.TenantAccount.Params;
using EasyIotSharp.Core.Dto.TenantAccount;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Services.TenantAccount.Impl
{
    public class SoldierService:ServiceBase, ISoldierService
    {
        //private readonly

        public SoldierService()
        {

        }

        public async Task<SoldierDto> GetSoldier(int id)
        {
            return new SoldierDto();
        }

        public async Task<PagedResultDto<SoldierDto>> QuerySoldier(QuerySoldierInput input)
        {
            return new PagedResultDto<SoldierDto>();
        }

        public async Task InsertSoldier(InsertSoldierInput input)
        {

        }

        public async Task UpdateSoldier(UpdateSoldierInput input)
        {

        }

        public async Task UpdateSoldierIsEnable(UpdateSoldierIsEnableInput input)
        {

        }

        public async Task DeleteSoldier(DeleteSoldierInput input)
        {

        }
    }
}
