using EasyIotSharp.Core.Dto.Project;
using EasyIotSharp.Core.Dto.Project.Params;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Services.Project
{
    public interface IGatewayProtocolService
    {
        /// <summary>
        /// 通过id获取一条网关协议信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GatewayProtocolDto> GetGatewayProtocol(string id);

        /// <summary>
        /// 根据条件分页查询网关协议列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<GatewayProtocolDto>> QueryGatewayProtocol(QueryGatewayProtocolInput input);

        /// <summary>
        /// 添加一条网关协议信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task InsertGatewayProtocol(InsertGatewayProtocolInput input);

        /// <summary>
        /// 通过id修改一条网关协议信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateGatewayProtocol(UpdateGatewayProtocolInput input);

        /// <summary>
        /// 通过id删除一条网关协议信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteGatewayProtocol(DeleteGatewayProtocolInput input);
    }
}
