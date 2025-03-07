using EasyIotSharp.Core.Dto.Project.Params;
using EasyIotSharp.Core.Dto.Project;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Services.Project
{
    public interface ISensorPointTypeQuotaService
    {
        /// <summary>
        /// 通过id获取一条传感器类型指标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SensorPointTypeQuotaDto> GetSensorPointTypeQuota(string id);

        /// <summary>
        /// 根据条件分页查询传感器类型指标列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SensorPointTypeQuotaDto>> QuerySensorPointTypeQuota(QuerySensorPointTypeQuotaInput input);

        /// <summary>
        /// 添加一条传感器类型指标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task InsertSensorPointTypeQuota(InsertSensorPointTypeQuotaInput input);

        /// <summary>
        /// 通过id修改一条传感器类型指标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateSensorPointTypeQuota(UpdateSensorPointTypeQuotaInput input);

        /// <summary>
        /// 通过id删除一条传感器类型指标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteSensorPointTypeQuota(DeleteSensorPointTypeQuotaInput input);
    }
}
