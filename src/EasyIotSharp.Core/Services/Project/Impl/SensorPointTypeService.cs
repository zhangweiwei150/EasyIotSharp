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
    public class SensorPointTypeService:ServiceBase,ISensorPointTypeService
    {
        private readonly ISensorPointTypeRepository _sensorPointTypeRepository;

        public SensorPointTypeService(ISensorPointTypeRepository sensorPointTypeRepository)
        {
            _sensorPointTypeRepository = sensorPointTypeRepository;
        }

        public async Task<SensorPointTypeDto> GetSensorPointType(string id)
        {
            var info = await _sensorPointTypeRepository.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == false);
            return info.MapTo<SensorPointTypeDto>();
        }

        public async Task<PagedResultDto<SensorPointTypeDto>> QuerySensorPointType(QuerySensorPointTypeInput input)
        {
            var query = await _sensorPointTypeRepository.Query(ContextUser.TenantNumId, input.PageIndex, input.PageSize, input.isPage);
            int totalCount = query.totalCount;
            var list = query.items.MapTo<List<SensorPointTypeDto>>();

            return new PagedResultDto<SensorPointTypeDto>() { TotalCount = totalCount, Items = list };
        }

        public async Task InsertSensorPointType(InsertSensorPointTypeInput input)
        {
            var isExistName = await _sensorPointTypeRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Name == input.Name && x.IsDelete == false);
            if (isExistName.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "测点类型名称重复");
            }
            var model = new SensorPointType();
            model.Id = Guid.NewGuid().ToString().Replace("-", "");
            model.TenantNumId = ContextUser.TenantNumId;
            model.Name = input.Name;
            model.IsDelete = false;
            model.CreationTime = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            model.OperatorId = ContextUser.UserId;
            model.OperatorName = ContextUser.UserName;
            await _sensorPointTypeRepository.InsertAsync(model);
        }

        public async Task UpdateSensorPointType(UpdateSensorPointTypeInput input)
        {
            var info = await _sensorPointTypeRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Id == input.Id && x.IsDelete == false);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "测点类型不存在");
            }
            var isExistName = await _sensorPointTypeRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Name == input.Name && x.IsDelete == false);
            if (isExistName.IsNotNull() && isExistName.Id != input.Id)
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "测点类型名称重复");
            }
            info.Name = input.Name;
            info.UpdatedAt = DateTime.Now;
            info.OperatorId = ContextUser.UserId;
            info.OperatorName = ContextUser.UserName;
            await _sensorPointTypeRepository.UpdateAsync(info);
        }

        public async Task DeleteSensorPointType(DeleteSensorPointTypeInput input)
        {
            var info = await _sensorPointTypeRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Id == input.Id && x.IsDelete == false);
            if (info.IsNotNull())
            {
                await _sensorPointTypeRepository.DeleteByIdAsync(info.Id);
            }
        }
    }
}
