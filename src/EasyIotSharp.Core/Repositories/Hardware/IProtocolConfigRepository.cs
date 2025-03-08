using EasyIotSharp.Core.Domain.Hardware;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.Hardware
{
    public interface IProtocolConfigRepository : IMySqlRepositoryBase<ProtocolConfig, string>
    {
        /// <summary>
        /// 通过条件分页查询
        /// </summary>
        /// <param name="protocolId">协议id</param>
        /// <param name="keyword">label/placeholder/tag</param>
        /// <param name="tagType">标签类型  -1=全部  1=text  2=checkbox  3=number</param>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页多少条</param>
        /// <param name="isPage">是否分页</param>
        /// <returns></returns>
        Task<(int totalCount, List<ProtocolConfig> items)> Query(string protocolId,
                                                                 string keyword,
                                                                 TagTypeMenu tagType, 
                                                                 int pageIndex,
                                                                 int pageSize,
                                                                 bool isPage);
    }
}
