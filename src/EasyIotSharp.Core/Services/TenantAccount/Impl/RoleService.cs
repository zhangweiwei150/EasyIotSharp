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
            var query = await _roleRepository.Query(ContextUser.TenantNumId, input.Keyword, input.IsEnable, input.PageIndex, input.PageSize);
            int totalCount = query.totalCount;
            var list = query.items.MapTo<List<RoleDto>>();
            return new PagedResultDto<RoleDto>() { TotalCount = totalCount, Items = list };
        }

        public async Task InsertRole(InsertRoleInput input)
        {
            var isExist = await _roleRepository.FirstOrDefaultAsync(x => x.IsDelete == false && x.TenantNumId == ContextUser.TenantNumId && x.Name == input.Name);
            if (isExist.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "名字重复");
            }
            var roleMedel = new Role();
            roleMedel.Id = Guid.NewGuid().ToString().Replace("-", "");
            roleMedel.TenantNumId = ContextUser.TenantNumId;
            roleMedel.IsManager = 2;
            roleMedel.Name = input.Name;
            roleMedel.Remark = input.Remark;
            roleMedel.IsEnable = input.IsEnable;
            roleMedel.CreationTime = DateTime.Now;
            roleMedel.UpdatedAt = DateTime.Now;
            roleMedel.OperatorId = ContextUser.UserId;
            roleMedel.OperatorName = ContextUser.UserName;
            await _roleRepository.InsertAsync(roleMedel);

            //通过子集菜单id获取父级菜单id
            var parentMenus = GetParentMenus((await _menuRepository.Query(0, "", 1, 1, 999, false)).items, input.Menus);

            var menuIds = parentMenus.Select(x => x.Id).ToList();
            menuIds.AddRange(input.Menus);
            var roleMenuInsertList = new List<RoleMenu>();
            foreach (var item in menuIds)
            {
                roleMenuInsertList.Add(new RoleMenu()
                {
                    Id = Guid.NewGuid().ToString().Replace("-", ""),
                    TenantNumId = ContextUser.TenantNumId,
                    RoleId = roleMedel.Id,
                    MenuId = item,
                    CreationTime = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    OperatorId = ContextUser.UserId,
                    OperatorName = ContextUser.UserName
                });
            }
            if (roleMenuInsertList.Count>0)
            {
                await _roleMenuRepository.InserManyAsync(roleMenuInsertList);
            }
        }

        public async Task<string> InsertAdminRole(InsertAdminRoleInput input)
        {

            var roleMedel = new Role();
            roleMedel.Id = Guid.NewGuid().ToString().Replace("-", "");
            roleMedel.TenantNumId = ContextUser.TenantNumId;
            roleMedel.IsManager = 1;
            roleMedel.Name = input.Name;
            roleMedel.Remark = input.Remark;
            roleMedel.IsEnable = true;
            roleMedel.CreationTime = DateTime.Now;
            roleMedel.UpdatedAt = DateTime.Now;
            roleMedel.OperatorId = ContextUser.UserId;
            roleMedel.OperatorName = ContextUser.UserName;
            await _roleRepository.InsertAsync(roleMedel);
            //创建一个管理员角色，包含所有非系统级别启用的菜单
            var menus = await _menuRepository.Query(2, "", 1, 0, 0, false);
            var roleMenuInsertList = new List<RoleMenu>();
            foreach (var item in menus.items)
            {
                roleMenuInsertList.Add(new RoleMenu()
                {
                    Id = Guid.NewGuid().ToString().Replace("-", ""),
                    TenantNumId = ContextUser.TenantNumId,
                    RoleId = roleMedel.Id,
                    MenuId = item.Id,
                    CreationTime = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    OperatorId = ContextUser.UserId,
                    OperatorName = ContextUser.UserName
                });
            }
            if (roleMenuInsertList.Count > 0)
            {
                await _roleMenuRepository.InserManyAsync(roleMenuInsertList);
            }
            return roleMedel.Id;
        }

        public async Task UpdateRole(UpdateRoleInput input)
        {
            var role = await _roleRepository.GetByIdAsync(input.Id);
            if (role.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "未找到指定的资源");
            }
            var isExist = await _roleRepository.FirstOrDefaultAsync(x => x.IsDelete == false && x.Id != input.Id && x.TenantNumId == ContextUser.TenantNumId && x.Name == input.Name);
            if (isExist.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "名字重复");
            }
            role.Name = input.Name;
            role.Remark = input.Remark;
            role.IsEnable = input.IsEnable;
            role.UpdatedAt = DateTime.Now;
            role.OperatorId = ContextUser.UserId;
            role.OperatorName = ContextUser.UserName;
            await _roleRepository.UpdateAsync(role);
            if (input.IsUpdateMenu==true)
            {
                //批量删除老的角色菜单表
                await _roleMenuRepository.DeleteManyByRoleId(role.Id);

                //通过子集菜单id获取父级菜单id
                var parentMenus = GetParentMenus(await _menuRepository.GetAllAsync(), input.Menus);
                var menuIds = parentMenus.Select(x => x.Id).ToList();
                menuIds.AddRange(input.Menus);
                //批量添加新的角色菜单表
                var roleMenuInsertList = new List<RoleMenu>();
                foreach (var item in input.Menus)
                {
                    roleMenuInsertList.Add(new RoleMenu()
                    {
                        Id = Guid.NewGuid().ToString().Replace("-", ""),
                        TenantNumId = ContextUser.TenantNumId,
                        RoleId = role.Id,
                        MenuId = item,
                        CreationTime = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        OperatorId = ContextUser.UserId,
                        OperatorName = ContextUser.UserName
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
                info.IsEnable = input.IsEnable;
                info.OperatorId = ContextUser.UserId;
                info.OperatorName = ContextUser.UserName;
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
                info.IsDelete = input.IsDelete;
                info.OperatorId = ContextUser.UserId;
                info.OperatorName = ContextUser.UserName;
                info.UpdatedAt = DateTime.Now;
                await _roleRepository.UpdateAsync(info);
            }
        }

        #region private

        // 通过子集ID数组获取所有父级直至顶级
        private List<Menu> GetParentMenus(List<Menu> menus, List<string> childIds)
        {
            var allParentMenus = new List<Menu>();

            foreach (var childId in childIds)
            {
                var parentMenus = new List<Menu>();
                var currentMenu = menus.FirstOrDefault(x => x.Id == childId);

                // 向上查找父级菜单
                while (currentMenu != null && !string.IsNullOrWhiteSpace(currentMenu.ParentId))
                {
                    currentMenu = menus.FirstOrDefault(x => x.Id == currentMenu.ParentId);
                    if (currentMenu != null)
                    {
                        parentMenus.Add(currentMenu);
                    }
                }

                // 反转列表，使得顶级菜单在最前面
                parentMenus.Reverse();

                // 将当前子集的父级菜单添加到总结果中
                allParentMenus.AddRange(parentMenus);
            }

            // 去重（如果多个子集有相同的父级菜单）
            allParentMenus = allParentMenus.Distinct().ToList();

            return allParentMenus;
        }

        #endregion
    }
}
