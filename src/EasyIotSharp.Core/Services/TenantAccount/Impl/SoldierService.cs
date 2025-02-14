using EasyIotSharp.Core.Dto.TenantAccount.Params;
using EasyIotSharp.Core.Dto.TenantAccount;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.Services.Dto;
using EasyIotSharp.Core.Repositories.TenantAccount;
using UPrime.AutoMapper;
using EasyIotSharp.Core.Repositories.Tenant;
using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Extensions;
using static EasyIotSharp.Core.GlobalConsts;
using EasyIotSharp.Core.Dto.Tenant;

namespace EasyIotSharp.Core.Services.TenantAccount.Impl
{
    public class SoldierService:ServiceBase, ISoldierService
    {
        private readonly ISoldierRepository _soldierRepository;
        private readonly ITenantRepository _tenantRepository;

        public SoldierService(ISoldierRepository soldierRepository,
                              ITenantRepository tenantRepository)
        {
            _soldierRepository = soldierRepository;
            _tenantRepository = tenantRepository;
        }

        public async Task<SoldierDto> GetSoldier(string id)
        {
            var info = await _soldierRepository.FirstOrDefaultAsync(x => x.IsDelete == false && x.Id == id);
            return info.MapTo<SoldierDto>();
        }

        public async Task<ValidateSoldierOutput> ValidateSoldier(ValidateSoldierInput input)
        {
            ValidateSoldierOutput res = new ValidateSoldierOutput();
            input.Mobile = input.Mobile.Trim();
            input.Password = input.Password.Trim();

            input.Password = AESHelper.Decrypt(input.Password, AES_KEY.Key);
            var md5Password = input.Password.Md5();

            //有同时存在不同的机构，不同职位同一个手机号的情况
            var user = await _soldierRepository.FirstOrDefaultAsync(x => x.Mobile == input.Mobile && x.Password == md5Password && x.IsDelete == false);
            if (user.IsNull())
            {
                res.Status = ValidateSoldierStatus.InvalidNameOrPassword;
                return res;
            }
            if (user.IsEnable==false)
            {
                res.Status = ValidateSoldierStatus.SoldiersIsDisable;
                return res;
            }
            var tenant = await _tenantRepository.FirstOrDefaultAsync(x => x.NumId == user.TenantNumId);
            if (tenant.IsNull())
            {
                res.Status = ValidateSoldierStatus.TenantIsNotExists;
            }
            if (tenant.IsDelete)
            {
                res.Status = ValidateSoldierStatus.TenantIsDeleted;
                return res;
            }
            if (tenant.IsFreeze)
            {
                res.Status = ValidateSoldierStatus.TenantIsFreeze;
                return res;
            }
            if (tenant.ContractEndTime < DateTime.Now)
            {
                res.Status = ValidateSoldierStatus.TenantIsExpired;
                return res;
            }
            user.LastLoginTime = DateTime.Now;
            //更新登录时间
            await _soldierRepository.UpdateAsync(user);
            res.Solider = user.MapTo<SoldierDto>();
            res.Tenant = tenant.MapTo<TenantDto>();
            res.Token = TokenExtensions.GenerateToken(res.Solider.Id);
            return res;
        }

        public async Task<PagedResultDto<SoldierDto>> QuerySoldier(QuerySoldierInput input)
        {
            var query = await _soldierRepository.Query(input.TenantId, input.Keyword, input.IsEnable, input.PageIndex, input.PageSize);
            int totalCount = query.totalCount;
            var list = query.items.MapTo<List<SoldierDto>>();
            return new PagedResultDto<SoldierDto>() { TotalCount = totalCount, Items = list };
        }

        public async Task<string> InsertSoldier(InsertSoldierInput input)
        {
            if (input.IsSuperAdmin == false)
            {
                var tenant = await _tenantRepository.FirstOrDefaultAsync(x => x.NumId == input.TenantNumId && x.IsDelete == false);
                if (tenant.IsNull())
                {
                    throw new BizException(BizError.BIND_EXCEPTION_ERROR, "租户信息不存在");
                }
                if (tenant.IsFreeze == true)
                {
                    throw new BizException(BizError.BIND_EXCEPTION_ERROR, "租户已冻结，请联系管理员");
                }
            }
            var isExistMobile = await _soldierRepository.FirstOrDefaultAsync(x => x.Mobile == input.Mobile && x.IsDelete == false);
            if (isExistMobile.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "手机号已存在");
            }
            var isExistUsername = await _soldierRepository.FirstOrDefaultAsync(x => x.Username == input.Username && x.TenantNumId == input.TenantNumId && x.IsDelete == false);
            if (isExistUsername.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "用户名重复");
            }
            var model = new Soldier();
            model.Id = Guid.NewGuid().ToString().Replace("-", "");
            model.TenantNumId = input.TenantNumId;
            model.IsSuperAdmin = input.IsSuperAdmin;
            model.IsManager = input.IsManager;
            model.Mobile = input.Mobile;
            model.Username = input.Username;
            model.Password = 6.GenerateRadomPassword().Md5();
            model.IsTest = false;
            model.Sex = input.Sex;
            model.IsEnable = true;
            model.Email = input.Email;
            await _soldierRepository.InsertAsync(model);
            return model.Id;
        }

        public async Task<string> UpdateSoldier(UpdateSoldierInput input)
        {
            var info = await _soldierRepository.GetByIdAsync(input.Id);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "用户信息不存在");
            }
            var isExistUsername = await _soldierRepository.FirstOrDefaultAsync(x => x.Username == input.Username && x.TenantNumId == info.TenantNumId && x.Id != input.Id && x.IsDelete == false);
            if (isExistUsername.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "用户名重复");
            }
            info.Username = input.Username;
            info.Sex = input.Sex;
            info.IsEnable = input.IsEnable;
            info.Email = input.Email;
            info.OperatorId = input.OperatorId;
            info.OperatorName = input.OperatorName;
            info.UpdatedAt = DateTime.Now;
            await _soldierRepository.UpdateAsync(info);
            return info.Id;
        }

        public async Task<string> UpdateSoldierIsEnable(UpdateSoldierIsEnableInput input)
        {
            var info = await _soldierRepository.GetByIdAsync(input.Id);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "用户信息不存在");
            }
            if (info.IsEnable!=input.IsEnable)
            {
                info.IsEnable = input.IsEnable;
                info.OperatorId = input.OperatorId;
                info.OperatorName = input.OperatorName;
                info.UpdatedAt = DateTime.Now;
                await _soldierRepository.UpdateAsync(info);
            }

            return info.Id;
        }

        public async Task DeleteSoldier(DeleteSoldierInput input)
        {
            var info = await _soldierRepository.GetByIdAsync(input.Id);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "用户信息不存在");
            }
            if (info.IsDelete != input.IsDelete)
            {
                info.IsDelete = input.IsDelete;
                info.UpdatedAt = DateTime.Now;
                info.OperatorId = input.OperatorId;
                info.OperatorName = input.OperatorName;
                await _soldierRepository.UpdateAsync(info);
            }
        }
    }
}
