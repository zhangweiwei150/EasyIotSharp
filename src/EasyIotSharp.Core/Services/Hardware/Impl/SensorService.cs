using EasyIotSharp.Core.Domain.Hardware;
using EasyIotSharp.Core.Dto.Hardware;
using EasyIotSharp.Core.Dto.Hardware.Params;
using EasyIotSharp.Core.Repositories.Hardware;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.AutoMapper;
using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Services.Hardware.Impl
{
    public class SensorService:ServiceBase,ISensorService
    {
        private readonly ISensorRepository _sensorRepository;

        public SensorService(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        public async Task<SensorDto> GetSensor(string id)
        {
            var info = await _sensorRepository.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == false);
            return info.MapTo<SensorDto>();
        }

        public async Task<PagedResultDto<SensorDto>> QuerySensor(QuerySensorInput input)
        {
            var query = await _sensorRepository.Query(ContextUser.TenantNumId,input.Keyword, input.PageIndex, input.PageSize, input.IsPage);
            int totalCount = query.totalCount;
            var list = query.items.MapTo<List<SensorDto>>();

            return new PagedResultDto<SensorDto>() { TotalCount = totalCount, Items = list };
        }

        public async Task InsertSensor(InsertSensorInput input)
        {
            var isExistBriefName = await _sensorRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.BriefName == input.BriefName && x.IsDelete == false);
            if (isExistBriefName.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器简称重复");
            }
            var model = new Sensor();
            model.Id = Guid.NewGuid().ToString().Replace("-", "");
            model.TenantNumId = ContextUser.TenantNumId;
            model.Name = input.Name;
            model.BriefName = input.BriefName;
            model.Supplier=input.Supplier;
            model.SensorModel = input.SensorModel;
            model.Remark = input.Remark;
            model.IsDelete = false;
            model.CreationTime = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            model.OperatorId = ContextUser.UserId;
            model.OperatorName = ContextUser.UserName;
            await _sensorRepository.InsertAsync(model);
        }

        public async Task UpdateSensor(UpdateSensorInput input)
        {
            var info = await _sensorRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Id == input.Id && x.IsDelete == false);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器不存在");
            }
            var isExistBriefName = await _sensorRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.BriefName == input.BriefName && x.IsDelete == false);
            if (isExistBriefName.IsNotNull() && isExistBriefName.Id != input.Id)
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器名称重复");
            }
            info.Name = input.Name;
            info.BriefName = input.BriefName;
            info.Supplier = input.Supplier;
            info.SensorModel = input.SensorModel;
            info.Remark = input.Remark;
            info.UpdatedAt = DateTime.Now;
            info.OperatorId = ContextUser.UserId;
            info.OperatorName = ContextUser.UserName;
            await _sensorRepository.UpdateAsync(info);
        }

        public async Task DeleteSensor(DeleteSensorInput input)
        {
            var info = await _sensorRepository.GetByIdAsync(input.Id);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器类型不存在");
            }
            if (info.IsDelete != input.IsDelete)
            {
                info.IsDelete = input.IsDelete;
                info.UpdatedAt = DateTime.Now;
                info.OperatorId = ContextUser.UserId;
                info.OperatorName = ContextUser.UserName;
                await _sensorRepository.UpdateAsync(info);
            }
        }
    }
}
