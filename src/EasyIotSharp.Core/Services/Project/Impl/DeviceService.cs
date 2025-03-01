using EasyIotSharp.Core.Domain.Proejct;
using EasyIotSharp.Core.Dto.Project;
using EasyIotSharp.Core.Dto.Project.Params;
using EasyIotSharp.Core.Repositories.Project;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.AutoMapper;
using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Services.Project.Impl
{
    public class DeviceService:ServiceBase,IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<DeviceDto> GetDevice(string id)
        {
            var info = await _deviceRepository.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == false);
            return info.MapTo<DeviceDto>();
        }

        public async Task<PagedResultDto<DeviceDto>> QueryDevice(QueryDeviceInput input)
        {
            var query = await _deviceRepository.Query(ContextUser.TenantNumId, input.Keyword, input.State, input.ProtocolId, input.ProjectId, input.PageIndex, input.PageSize);
            int totalCount = query.totalCount;
            var list = query.items.MapTo<List<DeviceDto>>();

            return new PagedResultDto<DeviceDto>() { TotalCount = totalCount, Items = list };
        }

        public async Task InsertDevice(InsertDeviceInput input)
        {
            var isExistName = await _deviceRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Name == input.Name && x.ProjectId == input.ProjectId && x.IsDelete == false);
            if (isExistName.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "设备名称重复");
            }
            var model = new Device();
            model.Id = Guid.NewGuid().ToString().Replace("-", "");
            model.TenantNumId = ContextUser.TenantNumId;
            model.Name = input.Name;
            model.State = 0;
            model.ProtocolId = input.ProtocolId;
            model.ProjectId = input.ProjectId;
            model.IsDelete = false;
            model.CreationTime = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            model.OperatorId = ContextUser.UserId;
            model.OperatorName = ContextUser.UserName;
            await _deviceRepository.InsertAsync(model);
        }

        public async Task UpdateDevice(UpdateDeviceInput input)
        {
            var info = await _deviceRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Id == input.Id && x.IsDelete == false);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "设备不存在");
            }
            var isExistName = await _deviceRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Name == input.Name && x.ProjectId == info.ProjectId && x.IsDelete == false);
            if (isExistName.IsNotNull() && isExistName.Id != input.Id)
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "设备名称重复");
            }
            info.Name = input.Name;
            info.State = info.State;
            info.ProtocolId = input.ProtocolId;
            info.ProjectId = input.ProjectId;
            info.UpdatedAt = DateTime.Now;
            info.OperatorId = ContextUser.UserId;
            info.OperatorName = ContextUser.UserName;
            await _deviceRepository.UpdateAsync(info);
        }

        public async Task DeleteDevice(DeleteDeviceInput input)
        {
            var info = await _deviceRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Id == input.Id && x.IsDelete == false);
            if (info.IsDelete != input.IsDelete)
            {
                info.IsDelete = input.IsDelete;
                info.UpdatedAt = DateTime.Now;
                info.OperatorId = ContextUser.UserId;
                info.OperatorName = ContextUser.UserName;
                await _deviceRepository.UpdateAsync(info);
            }
        }
    }
}
