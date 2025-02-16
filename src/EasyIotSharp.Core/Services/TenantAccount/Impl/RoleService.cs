using EasyIotSharp.Core.Dto.TenantAccount;
using EasyIotSharp.Core.Dto.TenantAccount.Params;
using EasyIotSharp.Core.Repositories.TenantAccount;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.AutoMapper;
using UPrime.Services.Dto;
using EasyIotSharp.Core.Domain.TenantAccount;

namespace EasyIotSharp.Core.Services.TenantAccount.Impl
{
    public class RoleService :ServiceBase,IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IMenuRepository _menuRepository;

        public RoleService(IRoleRepository roleRepository,
                           IRoleMenuRepository roleMenuRepository,
                           IMenuRepository menuRepository)
        {
            _roleRepository = roleRepository;
            _roleMenuRepository = roleMenuRepository;
            _menuRepository = menuRepository;
        }

        public async Task<GetRoleMenuOutput> GetRoleMenu(string id)
        {
            var role = await _roleRepository.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == false);
            if (role.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR,"未请求到指定的资源");
            }
            GetRoleMenuOutput output = new GetRoleMenuOutput();
            output = role.MapTo<GetRoleMenuOutput>();
            output.Menus = new List<MenuDto>();
            var roleMenus = await _roleMenuRepository.GetListAsync(x => x.RoleId == id);
            if (roleMenus.Count>0)
            {
                var menus = await _menuRepository.QueryByIds(roleMenus.Select(x => x.Id).ToList());
                output.Menus = menus.MapTo<List<MenuDto>>();
            }
            return output;
        }

        public async Task<PagedResultDto<RoleDto>> QueryRole(QueryRoleInput input)
        {
            var query = await _roleRepository.Query(input.TenantNumId, input.Keyword, input.IsEnable, input.PageIndex, input.PageSize);
            int totalCount = query.totalCount;
            var list = query.items.MapTo<List<RoleDto>>();
            return new PagedResultDto<RoleDto>() { TotalCount = totalCount, Items = list };
        }

        public async Task InsertRole(InsertRoleInput input)
        {
            var isExist = await _roleRepository.FirstOrDefaultAsync(x => x.IsDelete == false && x.TenantNumId == input.TenantNumId && x.Name == input.Name);
            if (isExist.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "名字重复");
            }
            var roleMedel = new Role();
            roleMedel.Id = Guid.NewGuid().ToString().Replace("-", "");
            roleMedel.TenantNumId = input.TenantNumId;
            roleMedel.Name = input.Name;
            roleMedel.Remark = input.Remark;
            roleMedel.IsEnable = input.IsEnable;
            roleMedel.CreationTime = DateTime.Now;
            roleMedel.UpdatedAt = DateTime.Now;
            roleMedel.OperatorId = input.OperatorId;
            roleMedel.OperatorName = input.OperatorName;
            await _roleRepository.InsertAsync(roleMedel);
            var roleMenuInsertList = new List<RoleMenu>();
            foreach (var item in input.Menus)
            {
                roleMenuInsertList.Add(new RoleMenu()
                {
                    Id = Guid.NewGuid().ToString().Replace("-", ""),
                    TenantNumId = input.TenantNumId,
                    RoleId = roleMedel.Id,
                    MenuId = item.Id,
                    CreationTime = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    OperatorId = input.OperatorId,
                    OperatorName = input.OperatorName
                });
            }
            if (roleMenuInsertList.Count>0)
            {
                await _roleMenuRepository.InserManyAsync(roleMenuInsertList);
            }
        }

        public async Task UpdateRole(UpdateRoleInput input)
        {
            var role = await _roleRepository.GetByIdAsync(input.Id);
            if (role.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "未找到指定的资源");
            }
            var isExist = await _roleRepository.FirstOrDefaultAsync(x => x.IsDelete == false && x.Id != input.Id && x.TenantNumId == input.TenantNumId && x.Name == input.Name);
            if (isExist.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "名字重复");
            }
            role.Name = input.Name;
            role.Remark = input.Remark;
            role.IsEnable = input.IsEnable;
            role.UpdatedAt = DateTime.Now;
            role.OperatorId = input.OperatorId;
            role.OperatorName = input.OperatorName;
            await _roleRepository.UpdateAsync(role);
            if (input.IsUpdateMenu==true)
            {
                //批量删除老的角色菜单表
                await _roleMenuRepository.DeleteManyByRoleId(role.Id);

                //批量添加新的角色菜单表
                var roleMenuInsertList = new List<RoleMenu>();
                foreach (var item in input.Menus)
                {
                    roleMenuInsertList.Add(new RoleMenu()
                    {
                        Id = Guid.NewGuid().ToString().Replace("-", ""),
                        TenantNumId = input.TenantNumId,
                        RoleId = role.Id,
                        MenuId = item.Id,
                        CreationTime = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        OperatorId = input.OperatorId,
                        OperatorName = input.OperatorName
                    });
                }
                if (roleMenuInsertList.Count > 0)
                {
                    await _roleMenuRepository.InserManyAsync(roleMenuInsertList);
                }
            }   
        }

        public async Task UpdateIsEnableRole(UpdateIsEnableRoleInput input)
        {
            var info = await _roleRepository.GetByIdAsync(input.Id);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "未找到指定数据");
            }
            if (info.IsEnable != input.IsEnable)
            {
                info.IsEnable = info.IsEnable;
                info.OperatorId = input.OperatorId;
                info.OperatorName = input.OperatorName;
                info.UpdatedAt = DateTime.Now;
                await _roleRepository.UpdateAsync(info);
            }
        }

        public async Task DeleteRole(DeleteRoleInput input)
        {
            var info = await _roleRepository.GetByIdAsync(input.Id);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "未找到指定数据");
            }
            if (info.IsDelete != input.IsDelete)
            {
                info.IsDelete = info.IsDelete;
                info.OperatorId = input.OperatorId;
                info.OperatorName = input.OperatorName;
                info.UpdatedAt = DateTime.Now;
                await _roleRepository.UpdateAsync(info);
            }
        }
    }
}
