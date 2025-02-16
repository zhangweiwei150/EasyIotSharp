using EasyIotSharp.Core.Repositories.Tenant;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyIotSharp.Core.Dto.Tenant.Params;
using EasyIotSharp.Core.Dto.Tenant;
using UPrime.Services.Dto;
using UPrime.AutoMapper;
using EasyIotSharp.Core.Repositories.TenantAccount;
using EasyIotSharp.Core.Services.TenantAccount;
using EasyIotSharp.Core.Dto.TenantAccount.Params;

namespace EasyIotSharp.Core.Services.Tenant.Impl
{
    public class TenantService:ServiceBase, ITenantService
    {
        private readonly ITenantRepository _tenantRepository;

        private readonly ISoldierService _soldierService;

        public TenantService(ITenantRepository tenantRepository,
                             ISoldierService soldierService)
        {
            _tenantRepository = tenantRepository;
            _soldierService = soldierService;
        }

        public async Task<TenantDto> GetTenant(string id)
        {
            var info = await _tenantRepository.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == false);
            return info.MapTo<TenantDto>();
        }

        public async Task<PagedResultDto<TenantDto>> QueryTenant(QueryTenantInput input)
        {
            var query = await _tenantRepository.Query(input.Keyword, input.ExpiredType, input.ContractStartTime, input.ContractEndTime, input.IsFreeze, input.PageIndex, input.PageSize);
            int totalCount = query.toalCount;
            var list = query.items.MapTo<List<TenantDto>>();
            return new PagedResultDto<TenantDto>() { TotalCount = totalCount, Items = list };
        }

        public async Task InsertTenant(InsertTenantInput input)
        {
            var info = await _tenantRepository.FirstOrDefaultAsync(x => x.Name == input.Name && x.IsDelete == false);
            if (info.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "租户名称重复");
            }
            int numId= (await _tenantRepository.CountAsync()) + 1;
            //创建租户管理员账号
            string managerId = await _soldierService.InsertSoldier(new InsertSoldierInput()
            {
                TenantNumId = numId,
                IsSuperAdmin=true,
                IsManager=1,
                Mobile=input.Mobile,
                Username=input.Owner,
                IsTest=false,
                Sex=-1,
                IsEnable=true,
                Email=""
            });


            var model = new EasyIotSharp.Core.Domain.Tenant.Tenant();
            model.Id = Guid.NewGuid().ToString().Replace("-", "");
            model.NumId = numId;
            model.Name = input.Name;
            model.ContractName = input.ContractName;
            model.ContractOwnerName = input.ContractOwnerName;
            model.ContractOwnerMobile = input.ContractOwnerMobile;
            model.ContractStartTime = input.ContractStartTime;
            model.ContractEndTime = input.ContractEndTime;
            model.Mobile = input.Mobile;
            model.StoreLogoUrl = input.StoreLogoUrl;
            model.Remark = input.Remark;
            model.Email = input.Email;
            model.VersionTypeId = input.VersionTypeId;
            model.IsFreeze = input.IsFreeze;
            model.FreezeDes = input.FreezeDes;
            model.ManagerId = managerId;
            model.IsDelete = false;
            model.CreationTime = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            model.OperatorId = input.OperatorId;
            model.OperatorName = input.OperatorName;
            await _tenantRepository.InsertAsync(model);
        }

        public async Task UpdateTenant(UpdateTenantInput input)
        {
            var info = await _tenantRepository.GetByIdAsync(input.Id);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "未找到指定数据");
            }
            var isExist = await _tenantRepository.FirstOrDefaultAsync(x => x.Name == input.Name && x.Id != input.Id && x.IsDelete == false);
            if (isExist.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "租户名称重复");
            }
            info.Name = input.Name;
            info.ContractName = input.ContractName;
            info.ContractOwnerName = input.ContractOwnerName;
            info.ContractOwnerMobile = input.ContractOwnerMobile;
            info.ContractStartTime = input.ContractStartTime;
            info.ContractEndTime = input.ContractEndTime;
            info.Mobile = input.Mobile;
            info.StoreLogoUrl = input.StoreLogoUrl;
            info.Remark = input.Remark;
            info.Email = input.Email;
            info.VersionTypeId = input.VersionTypeId;
            info.IsFreeze = input.IsFreeze;
            info.FreezeDes = input.FreezeDes;
            info.UpdatedAt = DateTime.Now;
            info.OperatorId = input.OperatorId;
            info.OperatorName = input.OperatorName;
            await _tenantRepository.UpdateAsync(info);
        }

        public async Task UpdateIsFreezeTenant(UpdateIsFreezeTenantTenantInput input)
        {
            var info = await _tenantRepository.GetByIdAsync(input.Id);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "未找到指定数据");
            }
            if (info.IsFreeze!= input.IsFreeze)
            {
                info.IsFreeze = input.IsFreeze;
                info.FreezeDes = input.FreezeDes;
                info.UpdatedAt = DateTime.Now;
                info.OperatorId = input.OperatorId;
                info.OperatorName = input.OperatorName;
                await _tenantRepository.UpdateAsync(info);
            }
        }

        public async Task DeleteTenant(DeleteTenantInput input)
        {
            var info = await _tenantRepository.GetByIdAsync(input.Id);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "未找到指定数据");
            }
            if (info.IsDelete != input.IsDelete)
            {
                info.IsDelete = input.IsDelete;
                info.UpdatedAt = DateTime.Now;
                info.OperatorId = input.OperatorId;
                info.OperatorName = input.OperatorName;
                await _tenantRepository.UpdateAsync(info);
            }
        }
    }
}
