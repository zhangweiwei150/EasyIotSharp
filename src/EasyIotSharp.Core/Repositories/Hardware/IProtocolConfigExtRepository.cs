using EasyIotSharp.Core.Domain.Hardware;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.Hardware
{
    public interface IProtocolConfigExtRepository : IMySqlRepositoryBase<ProtocolConfigExt, string>
    {
        /// <summary>
        /// 通过配置id集合查询配置扩展列表
        /// </summary>
        /// <param name="configIds"></param>
        /// <returns></returns>
        Task<List<ProtocolConfigExt>> QueryByConfigIds(List<string> configIds);

        /// <summary>
        /// 通过协议配置id批量删除配置扩展
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        Task<int> DeleteManyByConfigId(string configId);
    }
}
