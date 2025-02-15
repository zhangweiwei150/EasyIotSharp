using EasyIotSharp.Core.Dto.TenantAccount;
using EasyIotSharp.Core.Dto.TenantAccount.Params;
using System.Threading.Tasks;
using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Services.TenantAccount
{
    public interface IRoleService
    {
        /// <summary>
        /// 通过id查询一条角色菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GetRoleMenuOutput> GetRoleMenu(string id);

        /// <summary>
        /// 通过条件分页查询角色列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<RoleDto>> QueryRole(QueryRoleInput input);

        /// <summary>
        /// 添加一条角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task InsertRole(InsertRoleInput input);

        /// <summary>
        /// 通过id修改一条角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateRole(UpdateRoleInput input);

        /// <summary>
        /// 通过id修改一条菜单是否启用状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateIsEnableRole(UpdateIsEnableRoleInput input);
        /// <summary>
        /// 通过id获取一条角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteRole(DeleteRoleInput input);
    }
}
