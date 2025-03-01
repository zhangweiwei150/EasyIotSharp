using EasyIotSharp.Core.Domain.Proejct;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.Project
{
    public interface IProtocolRepository : IMySqlRepositoryBase<Protocol, string>
    {
        /// <summary>
        /// 通过条件分页查询项目分类列表
        /// </summary>
        /// <param name="isEnable">是否启用 -1=不参与查询 1=启用  2=禁用</param>
        /// <param name="keyword">协议名称/描述</param>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页多少条数据</param>
        /// <param name="isPage">是否分页</param>
        /// <returns></returns>
        Task<(int totalCount, List<Protocol> items)> Query(int isEnable,
                                                           string keyword,
                                                           int pageIndex,
                                                           int pageSize,
                                                           bool isPage);
    }
}
