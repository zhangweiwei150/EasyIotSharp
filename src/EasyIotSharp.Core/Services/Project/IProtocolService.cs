using EasyIotSharp.Core.Dto.Project;
using EasyIotSharp.Core.Dto.Project.Params;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Services.Project
{
    public interface IProtocolService
    {
        /// <summary>
        /// 通过id获取一条协议信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProtocolDto> GetProtocol(string id);

        /// <summary>
        /// 根据条件分页查询协议列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProtocolDto>> QueryProtocol(QueryProtocolInput input);

        /// <summary>
        /// 添加一条协议信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task InsertProtocol(InsertProtocolInput input);

        /// <summary>
        /// 通过id修改一条协议信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateProtocol(UpdateProtocolInput input);

        /// <summary>
        /// 通过id删除一条协议信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteProtocol(DeleteProtocolInput input);
    }
}
