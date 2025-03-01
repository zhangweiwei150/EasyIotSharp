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
    public class SensorPointService:ServiceBase,ISensorPointService
    {
        private readonly ISensorPointRepository _sensorPointRepository;

        public SensorPointService(ISensorPointRepository sensorPointRepository)
        {
            _sensorPointRepository = sensorPointRepository;
        }

        public async Task<SensorPointDto> GetSensorPoint(string id)
        {
            var info = await _sensorPointRepository.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == false);
            return info.MapTo<SensorPointDto>();
        }

        public async Task<PagedResultDto<SensorPointDto>> QuerySensorPoint(QuerySensorPointInput input)
        {
            var query = await _sensorPointRepository.Query(ContextUser.TenantNumId, input.Keyword,input.ProjectId,input.ClassificationId,input.DeviceId,input.SensorTypeId, input.PageIndex, input.PageSize, input.IsPage);
            int totalCount = query.totalCount;
            var list = query.items.MapTo<List<SensorPointDto>>();

            return new PagedResultDto<SensorPointDto>() { TotalCount = totalCount, Items = list };
        }

        public async Task InsertSensorPoint(InsertSensorPointInput input)
        {
            var isExistName = await _sensorPointRepository.FirstOrDefaultAsync(x => x.Name == input.Name && x.TenantNumId == ContextUser.TenantNumId && x.ProjectId == input.ProjectId && x.DeviceId == input.DeviceId && x.IsDelete == false);
            if (isExistName.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "测点名称重复");
            }
            var model = new SensorPoint();
            model.Id = Guid.NewGuid().ToString().Replace("-", "");
            model.TenantNumId = ContextUser.TenantNumId;
            model.Name = input.Name;
            model.ProjectId = input.ProjectId;
            model.ClassificationId = input.ClassificationId;
            model.DeviceId = input.DeviceId;
            model.SensorTypeId = input.SensorTypeId;
            model.IsDelete = false;
            model.CreationTime = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            model.OperatorId = ContextUser.UserId;
            model.OperatorName = ContextUser.UserName;
            await _sensorPointRepository.InsertAsync(model);
        }

        public async Task UpdateSensorPoint(UpdateSensorPointInput input)
        {
            var info = await _sensorPointRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Id == input.Id && x.IsDelete == false);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "测点不存在");
            }
            var isExistName = await _sensorPointRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Name == input.Name && x.ProjectId == info.ProjectId &&x.DeviceId==input.DeviceId && x.IsDelete == false);
            if (isExistName.IsNotNull() && isExistName.Id != input.Id)
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "测点名称重复");
            }
            info.Name = input.Name;
            info.ProjectId = info.ProjectId;
            info.ClassificationId = input.ClassificationId;
            info.DeviceId = input.DeviceId;
            info.SensorTypeId = input.SensorTypeId;
            info.UpdatedAt = DateTime.Now;
            info.OperatorId = ContextUser.UserId;
            info.OperatorName = ContextUser.UserName;
            await _sensorPointRepository.UpdateAsync(info);
        }

        public async Task DeleteSensorPoint(DeleteSensorPointInput input)
        {
            var info = await _sensorPointRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Id == input.Id && x.IsDelete == false);
            if (info.IsDelete != input.IsDelete)
            {
                info.IsDelete = input.IsDelete;
                info.UpdatedAt = DateTime.Now;
                info.OperatorId = ContextUser.UserId;
                info.OperatorName = ContextUser.UserName;
                await _sensorPointRepository.UpdateAsync(info);
            }
        }
    }
}
