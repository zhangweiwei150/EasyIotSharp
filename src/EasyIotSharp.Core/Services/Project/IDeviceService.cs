using EasyIotSharp.Core.Dto.Project;
using EasyIotSharp.Core.Dto.Project.Params;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Services.Project
{
    public interface IDeviceService
    {
        /// <summary>
        /// 通过id获取一条设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeviceDto> GetDevice(string id);

        /// <summary>
        /// 根据条件分页查询设备列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceDto>> QueryDevice(QueryDeviceInput input);

        /// <summary>
        /// 添加一条设备信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task InsertDevice(InsertDeviceInput input);

        /// <summary>
        /// 通过id修改一条设备信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateDevice(UpdateDeviceInput input);

        /// <summary>
        /// 通过id删除一条设备信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteDevice(DeleteDeviceInput input);
    }
}
