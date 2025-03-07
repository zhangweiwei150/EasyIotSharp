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
            var query = await _sensorPointTypeRepository.Query(ContextUser.TenantNumId,input.Keyword, input.PageIndex, input.PageSize, input.IsPage);
            int totalCount = query.totalCount;
            var list = query.items.MapTo<List<SensorPointTypeDto>>();

            return new PagedResultDto<SensorPointTypeDto>() { TotalCount = totalCount, Items = list };
        }

        public async Task InsertSensorPointType(InsertSensorPointTypeInput input)
        {
            var isExistBriefName = await _sensorPointTypeRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.BriefName == input.BriefName && x.IsDelete == false);
            if (isExistBriefName.IsNotNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "传感器简称重复");
            }
            var model = new SensorPointType();
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
            await _sensorPointTypeRepository.InsertAsync(model);
        }

        public async Task UpdateSensorPointType(UpdateSensorPointTypeInput input)
        {
            var info = await _sensorPointTypeRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.Id == input.Id && x.IsDelete == false);
            if (info.IsNull())
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "测点类型不存在");
            }
            var isExistBriefName = await _sensorPointTypeRepository.FirstOrDefaultAsync(x => x.TenantNumId == ContextUser.TenantNumId && x.BriefName == input.BriefName && x.IsDelete == false);
            if (isExistBriefName.IsNotNull() && isExistBriefName.Id != input.Id)
            {
                throw new BizException(BizError.BIND_EXCEPTION_ERROR, "测点类型名称重复");
            }
            info.Name = input.Name;
            info.BriefName = input.BriefName;
            info.Supplier = input.Supplier;
            info.SensorModel = input.SensorModel;
            info.Remark = input.Remark;
            info.UpdatedAt = DateTime.Now;
            info.OperatorId = ContextUser.UserId;
            info.OperatorName = ContextUser.UserName;
            await _sensorPointTypeRepository.UpdateAsync(info);
        }

        public async Task DeleteSensorPointType(DeleteSensorPointTypeInput input)
        {
            var info = await _sensorPointTypeRepository.GetByIdAsync(input.Id);
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
                await _sensorPointTypeRepository.UpdateAsync(info);
            }
        }
    }
}
