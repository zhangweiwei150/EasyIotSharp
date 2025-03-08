using EasyIotSharp.Core.Domain.Proejct;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.Project
{
    public interface IClassificationRepository : IMySqlRepositoryBase<Classification, string>
    {
        /// <summary>
        /// 通过条件分页查询项目分类列表
        /// </summary>
        /// <param name="tenantNumId">租户NumId</param>
        /// <param name="projectId">项目id</param>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页多少条数据</param>
        /// <param name="isPage">是否分页</param>
        /// <returns></returns>
        Task<(int totalCount, List<Classification> items)> Query(int tenantNumId,
                                                                 string projectId,
                                                                 int pageIndex,
                                                                 int pageSize,
                                                                 bool isPage);
        /// <summary>
        /// 根据ID集合查询项目分类列表
        /// </summary>
        /// <param name="ids">ID集合</param>
        /// <returns></returns>
        Task<List<Classification>> QueryByIds(List<string> ids);
    }
}
