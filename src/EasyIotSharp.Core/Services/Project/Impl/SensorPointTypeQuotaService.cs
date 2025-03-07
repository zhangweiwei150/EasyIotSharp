using System;
using System.Collections.Generic;
using System.Text;
using EasyIotSharp.Core.Dto.Project.Params;
using EasyIotSharp.Core.Dto.Project;
using System.Threading.Tasks;
using EasyIotSharp.Core.Repositories.Project;
using UPrime.Services.Dto;
using EasyIotSharp.Core.Repositories.Project.Impl;
using UPrime.AutoMapper;
using EasyIotSharp.Core.Domain.Proejct;

namespace EasyIotSharp.Core.Services.Project.Impl
{
    public class SensorPointTypeQuotaService : ServiceBase, ISensorPointTypeQuotaService
    {
        private readonly ISensorPointTypeQuotaRepository _sensorPointTypeQuotaRepository;

        public SensorPointTypeQuotaService(ISensorPointTypeQuotaRepository sensorPointTypeQuotaRepository)
        {
            _sensorPointTypeQuotaRepository = sensorPointTypeQuotaRepository;
        }

        public async Task<SensorPointTypeQuotaDto> GetSensorPointTypeQuota(string id)
        {
            var info = await _sensorPointTypeQuotaRepository.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == false);
            return info.MapTo<SensorPointTypeQuotaDto>();
        }

        public async Task<PagedResultDto<SensorPointTypeQuotaDto>> QuerySensorPointTypeQuota(QuerySensorPointTypeQuotaInput input)
        {
            var query = await _sensorPointTypeQuotaRepository.Query(ContextUser.TenantNumId, input.SensorPointTypeId, input.Keyword, input.DataType, input.PageIndex, input.PageSize, input.IsPage);
            int totalCount = query.totalCount;
            var list = query.items.MapTo<List<SensorPointTypeQuotaDto>>();

            return new PagedResultDto<SensorPointTypeQuotaDto>() { TotalCount = totalCount, Items = list };
        }

        public async Task InsertSensorPointTypeQuota(InsertSensorPointTypeQuotaInput input)
        {
            var isExistName = await _sensorPointTypeQuotaRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Name == input.Name && x.IsDelete == false);
            if (isExistName.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器类型指标名称重复");
            }
            var isExistIdentifier = await _sensorPointTypeQuotaRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Identifier == input.Identifier && x.IsDelete == false);
            if (isExistIdentifier.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器类型指标标识符重复");
            }
            var model = new SensorPointTypeQuota();
            model.Id = Guid.NewGuid().ToString().Replace("-", "");
            model.TenantNumId = ContextUser.TenantNumId;
            model.Name = input.Name;
            model.Identifier = input.Identifier;
            model.ControlsType = input.ControlsType;
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
            await _sensorPointTypeQuotaRepository.InsertAsync(model);
        }

        public async Task UpdateSensorPointTypeQuota(UpdateSensorPointTypeQuotaInput input)
        {
            var info = await _sensorPointTypeQuotaRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Id == input.Id && x.IsDelete == false);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器类型指标不存在");
            }
            var isExistName = await _sensorPointTypeQuotaRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Name == input.Name && x.IsDelete == false);
            if (isExistName.IsNotNull() && isExistName.Id != input.Id)
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器类型指标名称重复");
            }
            var isExistIdentifier = await _sensorPointTypeQuotaRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Identifier == input.Identifier && x.IsDelete == false);
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
            await _sensorPointTypeQuotaRepository.UpdateAsync(info);
        }

        public async Task DeleteSensorPointTypeQuota(DeleteSensorPointTypeQuotaInput input)
        {
            var info = await _sensorPointTypeQuotaRepository.GetByIdAsync(input.Id);
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
                await _sensorPointTypeQuotaRepository.UpdateAsync(info);
            }
        }
    }
}
