using EasyIotSharp.Core.Domain.TenantAccount;
using EasyIotSharp.Core.Dto.TenantAccount;
using EasyIotSharp.Core.Dto.TenantAccount.Params;
using EasyIotSharp.Core.Repositories.TenantAccount;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.AutoMapper;
using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Services.TenantAccount.Impl
{
    public class MenuService : ServiceBase, IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<MenuDto> GetMenu(string id)
        {
            var info = await _menuRepository.FirstOrDefaultAsync(x => x.IsDelete == false && x.Id == id);
            return info.MapTo<MenuDto>();
        }

        public async Task<PagedResultDto<MenuDto>> QueryMenu(QueryMenuInput input)
        {
            var query = await _menuRepository.Query(input.Keyword,input.IsEnable,input.PageIndex,input.PageSize);
            int totalCount = query.totalCount;
            var list = query.items.MapTo<List<MenuDto>>();
            return new PagedResultDto<MenuDto>() { TotalCount = totalCount, Items = list };
        }

        public async Task InsertMenu(InsertMenuInput input)
        {
            var isExist = await _menuRepository.FirstOrDefaultAsync(x => x.IsDelete == false && x.Type == input.Type && (x.Name == input.Name || x.Url == input.Url));
            if (isExist.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "菜单名称或路由重复");
            }
            var model = new Menu();
            model.Id= Guid.NewGuid().ToString().Replace("-", "");
            model.TenantNumId = 0;
            model.ParentId = input.ParentId;
            model.ParentName = input.ParentName;
            model.Name = input.Name;
            model.Icon = input.Icon;
            model.Url = input.Url;
            model.Type = input.Type;
            model.IsEnable = true;
            model.CreationTime = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            model.OperatorId = input.OperatorId;
            model.OperatorName = input.OperatorName;
            await _menuRepository.InsertAsync(model);
        }

        public async Task UpdateMenu(UpdateMenuInput input)
        {
            var info = await _menuRepository.GetByIdAsync(input.Id);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "未找到指定数据");
            }
            var isExist = await _menuRepository.FirstOrDefaultAsync(x => x.Id != input.Id && x.IsDelete == false && x.Type == info.Type && (x.Name == input.Name || x.Url == input.Url));
            if (isExist.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "菜单名称或路由重复");
            }
            info.Name = input.Name;
            info.Icon = input.Icon;
            info.Url = input.Url;
            info.IsEnable = input.IsEnable;
            info.OperatorId = input.OperatorId;
            info.OperatorName = input.OperatorName;
            info.UpdatedAt = DateTime.Now;
            await _menuRepository.UpdateAsync(info);
        }

        public async Task UpdateIsEnableMenu(UpdateIsEnableMenuInput input)
        {
            var info = await _menuRepository.GetByIdAsync(input.Id);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "未找到指定数据");
            }
            if (info.IsEnable!=input.IsEnable)
            {
                info.IsEnable = info.IsEnable;
                info.OperatorId = input.OperatorId;
                info.OperatorName = input.OperatorName;
                info.UpdatedAt = DateTime.Now;
                await _menuRepository.UpdateAsync(info);
            }
        }

        public async Task DeleteMenu(DeleteMenuInput input)
        {
            var info = await _menuRepository.GetByIdAsync(input.Id);
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
                await _menuRepository.UpdateAsync(info);
            }
        }
    }
}
