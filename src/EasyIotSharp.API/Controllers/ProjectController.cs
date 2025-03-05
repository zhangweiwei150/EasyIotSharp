using EasyIotSharp.API.Filters;
using EasyIotSharp.Core.Dto.Project;
using EasyIotSharp.Core.Dto.Project.Params;
using EasyIotSharp.Core.Services.Project;
using EasyIotSharp.Core.Services.Tenant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPrime.Services.Dto;
using UPrime.WebApi;

namespace EasyIotSharp.API.Controllers
{
    public class ProjectController : ApiControllerBase
    {
        private readonly IProjectBaseService _projectBaseService;
        private readonly IClassificationService _classificationService;
        private readonly IProtocolService _protocolService;
        private readonly IDeviceService _deviceService;
        private readonly ISensorPointService _sensorPointService;
        private readonly ISensorPointTypeService _sensorPointTypeService;

        public ProjectController()
        {
            _projectBaseService = UPrime.UPrimeEngine.Instance.Resolve<IProjectBaseService>();
            _classificationService = UPrime.UPrimeEngine.Instance.Resolve<IClassificationService>();
            _protocolService = UPrime.UPrimeEngine.Instance.Resolve<IProtocolService>();
            _deviceService = UPrime.UPrimeEngine.Instance.Resolve<IDeviceService>();
            _sensorPointService = UPrime.UPrimeEngine.Instance.Resolve<ISensorPointService>();
            _sensorPointTypeService = UPrime.UPrimeEngine.Instance.Resolve<ISensorPointTypeService>();
        }

        #region 项目

        /// <summary>
        /// 通过id获取一条项目信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("/Project/ProjectBase/Get")]
        [Authorize]
        public async Task<UPrimeResponse<ProjectBaseDto>> GetProjectBase(string id)
        {
            UPrimeResponse<ProjectBaseDto> res = new UPrimeResponse<ProjectBaseDto>();
            res.Result = await _projectBaseService.GetProjectBase(id);
            return res;
        }

        /// <summary>
        /// 通过条件分页查询项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/ProjectBase/Query")]
        [Authorize]
        public async Task<UPrimeResponse<PagedResultDto<ProjectBaseDto>>> QueryProjectBase([FromBody]QueryProjectBaseInput input)
        {
            UPrimeResponse<PagedResultDto<ProjectBaseDto>> res = new UPrimeResponse<PagedResultDto<ProjectBaseDto>>();
            res.Result = await _projectBaseService.QueryProjectBase(input);
            return res;
        }

        /// <summary>
        /// 添加一条项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/ProjectBase/Insert")]
        [Authorize]
        public async Task<UPrimeResponse> InsertProjectBase([FromBody]InsertProjectBaseInput input)
        {
            await _projectBaseService.InsertProjectBase(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一条项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/ProjectBase/Update")]
        [Authorize]
        public async Task<UPrimeResponse> UpdateProjectBase([FromBody] UpdateProjectBaseInput input)
        {
            await _projectBaseService.UpdateProjectBase(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一条项目信息状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/ProjectBase/UpdateState")]
        [Authorize]
        public async Task<UPrimeResponse> UpdateStateProjectBase([FromBody] UpdateStateProjectBaseInput input)
        {
            await _projectBaseService.UpdateStateProjectBase(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id删除一条项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/ProjectBase/Delete")]
        [Authorize]
        public async Task<UPrimeResponse> DeleteProjectBase([FromBody]DeleteProjectBaseInput input)
        {
            await _projectBaseService.DeleteProjectBase(input);
            return new UPrimeResponse();
        }

        #endregion

        #region 项目分类

        /// <summary>
        /// 通过id获取一条项目分类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("/Project/Classification/Get")]
        [Authorize]
        public async Task<UPrimeResponse<ClassificationDto>> GetClassification(string id)
        {
            UPrimeResponse<ClassificationDto> res = new UPrimeResponse<ClassificationDto>();
            res.Result = await _classificationService.GetClassification(id);
            return res;
        }

        /// <summary>
        /// 通过条件分页查询项目分类信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/Classification/Query")]
        [Authorize]
        public async Task<UPrimeResponse<PagedResultDto<ClassificationDto>>> QueryClassification([FromBody] QueryClassificationInput input)
        {
            UPrimeResponse<PagedResultDto<ClassificationDto>> res = new UPrimeResponse<PagedResultDto<ClassificationDto>>();
            res.Result = await _classificationService.QueryClassification(input);
            return res;
        }

        /// <summary>
        /// 添加一条项目分类信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/Classification/Insert")]
        [Authorize]
        public async Task<UPrimeResponse> InsertClassification([FromBody] InsertClassificationInput input)
        {
            await _classificationService.InsertClassification(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一条项目分类信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/Classification/Update")]
        [Authorize]
        public async Task<UPrimeResponse> UpdateClassification([FromBody] UpdateClassificationInput input)
        {
            await _classificationService.UpdateClassification(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id删除一条项目分类信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/Classification/Delete")]
        [Authorize]
        public async Task<UPrimeResponse> DeleteClassification([FromBody] DeleteClassificationInput input)
        {
            await _classificationService.DeleteClassification(input);
            return new UPrimeResponse();
        }

        #endregion

        #region 协议

        /// <summary>
        /// 通过id获取一条协议信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("/Project/Protocol/Get")]
        [Authorize]
        public async Task<UPrimeResponse<ProtocolDto>> GetProtocol(string id)
        {
            UPrimeResponse<ProtocolDto> res = new UPrimeResponse<ProtocolDto>();
            res.Result = await _protocolService.GetProtocol(id);
            return res;
        }

        /// <summary>
        /// 通过条件分页查询协议信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/Protocol/Query")]
        [Authorize]
        public async Task<UPrimeResponse<PagedResultDto<ProtocolDto>>> QueryProtocol([FromBody] QueryProtocolInput input)
        {
            UPrimeResponse<PagedResultDto<ProtocolDto>> res = new UPrimeResponse<PagedResultDto<ProtocolDto>>();
            res.Result = await _protocolService.QueryProtocol(input);
            return res;
        }

        /// <summary>
        /// 添加一条协议信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/Protocol/Insert")]
        [Authorize]
        public async Task<UPrimeResponse> InsertProtocol([FromBody] InsertProtocolInput input)
        {
            await _protocolService.InsertProtocol(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一条协议信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/Protocol/Update")]
        [Authorize]
        public async Task<UPrimeResponse> UpdateProtocol([FromBody] UpdateProtocolInput input)
        {
            await _protocolService.UpdateProtocol(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id删除一条协议信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/Protocol/Delete")]
        [Authorize]
        public async Task<UPrimeResponse> DeleteProtocol([FromBody] DeleteProtocolInput input)
        {
            await _protocolService.DeleteProtocol(input);
            return new UPrimeResponse();
        }

        #endregion

        #region 设备

        /// <summary>
        /// 通过id获取一条设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("/Project/Device/Get")]
        [Authorize]
        public async Task<UPrimeResponse<DeviceDto>> GetDevice(string id)
        {
            UPrimeResponse<DeviceDto> res = new UPrimeResponse<DeviceDto>();
            res.Result = await _deviceService.GetDevice(id);
            return res;
        }

        /// <summary>
        /// 通过条件分页查询设备信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/Device/Query")]
        [Authorize]
        public async Task<UPrimeResponse<PagedResultDto<DeviceDto>>> QueryDevice([FromBody] QueryDeviceInput input)
        {
            UPrimeResponse<PagedResultDto<DeviceDto>> res = new UPrimeResponse<PagedResultDto<DeviceDto>>();
            res.Result = await _deviceService.QueryDevice(input);
            return res;
        }

        /// <summary>
        /// 添加一条设备信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/Device/Insert")]
        [Authorize]
        public async Task<UPrimeResponse> InsertDevice([FromBody] InsertDeviceInput input)
        {
            await _deviceService.InsertDevice(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一条设备信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/Device/Update")]
        [Authorize]
        public async Task<UPrimeResponse> UpdateDevice([FromBody] UpdateDeviceInput input)
        {
            await _deviceService.UpdateDevice(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id删除一条设备信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/Device/Delete")]
        [Authorize]
        public async Task<UPrimeResponse> DeleteDevice([FromBody] DeleteDeviceInput input)
        {
            await _deviceService.DeleteDevice(input);
            return new UPrimeResponse();
        }

        #endregion

        #region 测点

        /// <summary>
        /// 通过id获取一条测点信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("/Project/SensorPoint/Get")]
        [Authorize]
        public async Task<UPrimeResponse<SensorPointDto>> GetSensorPoint(string id)
        {
            UPrimeResponse<SensorPointDto> res = new UPrimeResponse<SensorPointDto>();
            res.Result = await _sensorPointService.GetSensorPoint(id);
            return res;
        }

        /// <summary>
        /// 通过条件分页查询测点列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/SensorPoint/Query")]
        [Authorize]
        public async Task<UPrimeResponse<PagedResultDto<SensorPointDto>>> QuerySensorPoint([FromBody] QuerySensorPointInput input)
        {
            UPrimeResponse<PagedResultDto<SensorPointDto>> res = new UPrimeResponse<PagedResultDto<SensorPointDto>>();
            res.Result = await _sensorPointService.QuerySensorPoint(input);
            return res;
        }

        /// <summary>
        /// 添加一条测点信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/SensorPoint/Insert")]
        [Authorize]
        public async Task<UPrimeResponse> InsertSensorPoint([FromBody] InsertSensorPointInput input)
        {
            await _sensorPointService.InsertSensorPoint(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一条测点信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/SensorPoint/Update")]
        [Authorize]
        public async Task<UPrimeResponse> UpdateSensorPoint([FromBody] UpdateSensorPointInput input)
        {
            await _sensorPointService.UpdateSensorPoint(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id删除一条测点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/SensorPoint/Delete")]
        [Authorize]
        public async Task<UPrimeResponse> DeleteSensorPoint([FromBody] DeleteSensorPointInput input)
        {
            await _sensorPointService.DeleteSensorPoint(input);
            return new UPrimeResponse();
        }

        #endregion

        #region 测点类型

        /// <summary>
        /// 通过id获取一条测点类型信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("/Project/SensorPointType/Get")]
        [Authorize]
        public async Task<UPrimeResponse<SensorPointTypeDto>> GetSensorPointType(string id)
        {
            UPrimeResponse<SensorPointTypeDto> res = new UPrimeResponse<SensorPointTypeDto>();
            res.Result = await _sensorPointTypeService.GetSensorPointType(id);
            return res;
        }

        /// <summary>
        /// 通过条件分页查询项目分类信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/SensorPointType/Query")]
        [Authorize]
        public async Task<UPrimeResponse<PagedResultDto<SensorPointTypeDto>>> QuerySensorPointType([FromBody] QuerySensorPointTypeInput input)
        {
            UPrimeResponse<PagedResultDto<SensorPointTypeDto>> res = new UPrimeResponse<PagedResultDto<SensorPointTypeDto>>();
            res.Result = await _sensorPointTypeService.QuerySensorPointType(input);
            return res;
        }

        /// <summary>
        /// 添加一条测点类型信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/SensorPointType/Insert")]
        [Authorize]
        public async Task<UPrimeResponse> InsertSensorPointType([FromBody] InsertSensorPointTypeInput input)
        {
            await _sensorPointTypeService.InsertSensorPointType(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id修改一条测点类型信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/SensorPointType/Update")]
        [Authorize]
        public async Task<UPrimeResponse> UpdateSensorPointType([FromBody] UpdateSensorPointTypeInput input)
        {
            await _sensorPointTypeService.UpdateSensorPointType(input);
            return new UPrimeResponse();
        }

        /// <summary>
        /// 通过id删除一条项目分类信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/Project/SensorPointType/Delete")]
        [Authorize]
        public async Task<UPrimeResponse> DeleteSensorPointType([FromBody] DeleteSensorPointTypeInput input)
        {
            await _sensorPointTypeService.DeleteSensorPointType(input);
            return new UPrimeResponse();
        }

        #endregion
    }
}
