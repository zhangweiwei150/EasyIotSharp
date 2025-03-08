using EasyIotSharp.Core.Dto.Project;
using EasyIotSharp.Core.Dto.Project.Params;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Services.Project
{
    public interface IGatewayService
    {
        /// <summary>
        /// 通过id获取一条网关信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GatewayDto> GetGateway(string id);

        /// <summary>
        /// 根据条件分页查询网关列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<GatewayDto>> QueryGateway(QueryGatewayInput input);

        /// <summary>
        /// 添加一条网关信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task InsertGateway(InsertGatewayInput input);

        /// <summary>
        /// 通过id修改一条网关信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateGateway(UpdateGatewayInput input);

        /// <summary>
        /// 通过id删除一条网关信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteGateway(DeleteGatewayInput input);
    }
}
