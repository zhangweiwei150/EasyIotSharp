using System;
using System.Collections.Generic;
using System.Text;
using EasyIotSharp.Core.Dto.Hardware.Params;
using EasyIotSharp.Core.Dto.Hardware;
using System.Threading.Tasks;
using EasyIotSharp.Core.Repositories.Hardware;
using UPrime.Services.Dto;
using EasyIotSharp.Core.Repositories.Hardware.Impl;
using UPrime.AutoMapper;
using EasyIotSharp.Core.Domain.Hardware;

namespace EasyIotSharp.Core.Services.Hardware.Impl
{
    public class SensorQuotaService : ServiceBase, ISensorQuotaService
    {
        private readonly ISensorQuotaRepository _sensorQuotaRepository;

        public SensorQuotaService(ISensorQuotaRepository sensorQuotaRepository)
        {
            _sensorQuotaRepository = sensorQuotaRepository;
        }

        public async Task<SensorQuotaDto> GetSensorQuota(string id)
        {
            var info = await _sensorQuotaRepository.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == false);
            return info.MapTo<SensorQuotaDto>();
        }

        public async Task<PagedResultDto<SensorQuotaDto>> QuerySensorQuota(QuerySensorQuotaInput input)
        {
            var query = await _sensorQuotaRepository.Query(ContextUser.TenantNumId, input.SensorPointTypeId, input.Keyword, input.DataType, input.PageIndex, input.PageSize, input.IsPage);
            int totalCount = query.totalCount;
            var list = query.items.MapTo<List<SensorQuotaDto>>();

            return new PagedResultDto<SensorQuotaDto>() { TotalCount = totalCount, Items = list };
        }

        public async Task InsertSensorQuota(InsertSensorQuotaInput input)
        {
            var isExistName = await _sensorQuotaRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Name == input.Name && x.IsDelete == false);
            if (isExistName.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器类型指标名称重复");
            }
            var isExistIdentifier = await _sensorQuotaRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Identifier == input.Identifier && x.IsDelete == false);
            if (isExistIdentifier.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器类型指标标识符重复");
            }
            var model = new SensorQuota();
            model.Id = Guid.NewGuid().ToString().Replace("-", "");
            model.TenantNumId = ContextUser.TenantNumId;
            model.Name = input.Name;
            model.Identifier = input.Identifier;
            model.ControlsType = input.ControlsType;
            model.SensorId = input.SensorId;
            model.DataType = input.DataType;
            model.Unit = input.Unit;
            model.Precision = input.Precision;
            model.Remark = input.Remark;
            model.Sort = input.Sort;
            model.IsDelete = false;
            model.CreationTime = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            model.OperatorId = ContextUser.UserId;
            model.OperatorName = ContextUser.UserName;
            await _sensorQuotaRepository.InsertAsync(model);
        }

        public async Task UpdateSensorQuota(UpdateSensorQuotaInput input)
        {
            var info = await _sensorQuotaRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Id == input.Id && x.IsDelete == false);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器类型指标不存在");
            }
            var isExistName = await _sensorQuotaRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Name == input.Name && x.IsDelete == false);
            if (isExistName.IsNotNull() && isExistName.Id != input.Id)
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器类型指标名称重复");
            }
            var isExistIdentifier = await _sensorQuotaRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Identifier == input.Identifier && x.IsDelete == false);
            if (isExistIdentifier.IsNotNull() && isExistName.Id != input.Id)
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器类型指标标识符重复");
            }
            info.Name = input.Name;
            info.Identifier = input.Identifier;
            info.ControlsType = input.ControlsType;
            info.DataType = input.DataType;
            info.Unit = input.Unit;
            info.Precision = input.Precision;
            info.Remark = input.Remark;
            info.Sort = input.Sort;
            info.UpdatedAt = DateTime.Now;
            info.OperatorId = ContextUser.UserId;
            info.OperatorName = ContextUser.UserName;
            await _sensorQuotaRepository.UpdateAsync(info);
        }

        public async Task DeleteSensorQuota(DeleteSensorQuotaInput input)
        {
            var info = await _sensorQuotaRepository.GetByIdAsync(input.Id);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器类型指标不存在");
            }
            if (info.IsDelete != input.IsDelete)
            {
                info.IsDelete = input.IsDelete;
                info.UpdatedAt = DateTime.Now;
                info.OperatorId = ContextUser.UserId;
                info.OperatorName = ContextUser.UserName;
                await _sensorQuotaRepository.UpdateAsync(info);
            }
        }
    }
}
