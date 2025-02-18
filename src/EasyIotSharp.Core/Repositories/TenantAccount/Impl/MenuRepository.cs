using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Dto.TenantAccount;
using EasyIotSharp.Core.Repositories.Mysql;
using EasyIotSharp.Repositories.Mysql;
using LinqKit;

namespace EasyIotSharp.Core.Repositories.TenantAccount.Impl
{
    public class MenuRepository : MySqlRepositoryBase<Menu, string>, IMenuRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="databaseProvider">数据库提供者</param>
        public MenuRepository(SqlSugarDatabaseProvider databaseProvider) : base(databaseProvider)
        {
        }

        public async Task<(int totalCount, List<Menu> items)> Query(int isSugerAdmin, string keyword, int isEnable, int pageIndex, int pageSize, bool isPage = true)
        {
            // 初始化条件
            var predicate = PredicateBuilder.New<Menu>(m => m.IsDelete == false);

            if (isSugerAdmin>-1)
            {
                predicate = predicate.And(m => m.IsSuperAdmin == (isSugerAdmin == 1 ? true : false)) ;
            }

            // 关键字模糊查询
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                predicate = predicate.And(m => m.Name.Contains(keyword));
            }

            // 是否启用过滤
            if (isEnable > -1)
            {
                predicate = predicate.And(m => m.IsEnable == (isEnable == 1));
            }

            // 获取总记录数
            var totalCount = await CountAsync(predicate);
            if (totalCount == 0)
            {
                return (0, new List<Menu>());
            }
            if (isPage==true)
            {
                // 手动拼接排序和分页逻辑
                var query = GetDbClient().Queryable<Menu>().Where(predicate)
                                  .OrderByDescending(m => m.CreationTime) // 默认按 CreationTime 降序排序
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize);

                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
            else
            {
                // 手动拼接排序和分页逻辑
                var query = GetDbClient().Queryable<Menu>().Where(predicate)
                                  .OrderByDescending(m => m.CreationTime); // 默认按 CreationTime 降序排序

                // 查询数据
                var items = await query.ToListAsync();
                return (totalCount, items);
            }
        }

        /// <summary>
        /// 根据ID集合查询菜单
        /// </summary>
        /// <param name="ids">菜单ID集合</param>
        /// <returns>返回菜单列表</returns>
        public async Task<List<Menu>> QueryByIds(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<Menu>();
            }

            // 使用表达式构建查询条件
            var predicate = PredicateBuilder.New<Menu>(false); // 初始化为空条件
            foreach (var id in ids)
            {
                var tempId = id; // 避免闭包问题
                predicate = predicate.Or(m => m.Id == tempId);
            }

            // 查询数据
            var items = await GetListAsync(predicate);
            return items.ToList();
        }


        /// <summary>
        /// 构建菜单树形结构
        /// </summary>
        public List<MenuTreeDto> BuildMenuTree(List<MenuDto> menuList)
        {
            // 找到所有顶级菜单（ParentId 为 null 的菜单）
            var topLevelMenus = menuList.Where(menu => string.IsNullOrEmpty(menu.ParentId)).ToList();

            // 递归构建子菜单
            var tree = topLevelMenus.Select(menu => new MenuTreeDto
            {
                Id = menu.Id,
                CreationTime = menu.CreationTime,
                IsDelete = menu.IsDelete,
                DeleteTime = menu.DeleteTime,
                UpdatedAt = menu.UpdatedAt,
                OperatorId = menu.OperatorId,
                OperatorName = menu.OperatorName,
                ParentId = menu.ParentId,
                Name = menu.Name,
                Icon = menu.Icon,
                Url = menu.Url,
                Type = menu.Type,
                IsSuperAdmin = menu.IsSuperAdmin,
                IsEnable = menu.IsEnable,
                Children = GetChildren(menu.Id, menuList)
            }).ToList();

            return tree;
        }

        /// <summary>
        /// 递归获取子菜单
        /// </summary>
        private List<MenuTreeDto> GetChildren(string parentId, List<MenuDto> menuList)
        {
            var children = menuList.Where(menu => menu.ParentId == parentId).ToList();
            if (children.Count == 0)
            {
                return null; // 没有子菜单时返回 null
            }

            return children.Select(child => new MenuTreeDto
            {
                Id = child.Id,
                CreationTime = child.CreationTime,
                IsDelete = child.IsDelete,
                DeleteTime = child.DeleteTime,
                UpdatedAt = child.UpdatedAt,
                OperatorId = child.OperatorId,
                OperatorName = child.OperatorName,
                ParentId = child.ParentId,
                Name = child.Name,
                Icon = child.Icon,
                Url = child.Url,
                Type = child.Type,
                IsSuperAdmin = child.IsSuperAdmin,
                IsEnable = child.IsEnable,
                Children = GetChildren(child.Id, menuList) // 递归调用
            }).ToList();
        }
    }
}