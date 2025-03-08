using EasyIotSharp.Core.Dto.Hardware;
using EasyIotSharp.Core.Dto.Hardware.Params;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Services.Hardware
{
    public interface IProtocolConfigService
    {
        /// <summary>
        /// 通过id获取一条协议配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProtocolConfigDto> GetProtocolConfig(string id);

        /// <summary>
        /// 通过条件分页查询协议配置信息列表
        /// </summary>.
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<QueryProtocolConfigByProtocolIdOutput>> QueryProtocolConfig(QueryProtocolConfigInput input);

        /// <summary>
        /// 添加一条协议配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task InsertProtocolConfig(InsertProtocolConfigInput input);

        /// <summary>
        /// 通过id修改一条协议配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateProtocolConfig(UpdateProtocolConfigInput input);

        /// <summary>
        /// 通过id删除一条协议配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteProtocolConfig(DeleteProtocolConfigInput input);
    }
}
