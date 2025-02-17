using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Dto.TenantAccount.Params;
using EasyIotSharp.Core.Repositories.TenantAccount;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Services.TenantAccount.Impl
{
    public class SoldierRoleService:ServiceBase, ISoldierRoleService
    {
        private readonly ISoldierRepository _soldierRepository;
        private readonly ISoldierRoleRepository _soldierRoleRepository;

        public SoldierRoleService(ISoldierRepository soldierRepository,
                                  ISoldierRoleRepository soldierRoleRepository)
        {
            _soldierRepository = soldierRepository;
            _soldierRoleRepository = soldierRoleRepository;
        }

        public async Task UpdateSoldierRole(UpdateSoldierRoleInput input)
        {
            var soldier = await _soldierRepository.GetByIdAsync(input.SoldierId);
            if (soldier.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR,"未找到指定的资源");
            }
            //批量删除用户角色表
            await _soldierRoleRepository.DeleteManyBySoldierId(input.SoldierId);
            //批量添加用户角色表
            var soldierRoleInsertList = new List<SoldierRole>();
            foreach (var item in input.Roles)
            {
                soldierRoleInsertList.Add(new SoldierRole()
                {
                    Id = Guid.NewGuid().ToString().Replace("-", ""),
                    TenantNumId = soldier.TenantNumId,
                    SoldierId = soldier.Id,
                    RoleId = item.Id,
                    CreationTime = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    OperatorId = ContextUser.UserId,
                    OperatorName = ContextUser.UserName
                });
            }
            if (soldierRoleInsertList.Count > 0)
            {
                await _soldierRoleRepository.InserManyAsync(soldierRoleInsertList);
            }
        }
    }
}
