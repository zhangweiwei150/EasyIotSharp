using EasyIotSharp.Core.Domain.Proejct;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.Project
{
    public interface IProjectBaseRepository : IMySqlRepositoryBase<ProjectBase, string>
    {
        /// <summary>
        /// 通过条件分页查询项目列表
        /// </summary>
        /// <param name="tenantNumId">租户NumId</param>
        /// <param name="keyword">项目名称/备注</param>
        /// <param name="state">状态 -1=全部  0=初始化状态  1=正在运行状态</param>
        /// <param name="createStartTime">创建开始时间</param>
        /// <param name="createEndTime">创建结束时间</param>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页多少条数据</param>
        /// <returns></returns>
        Task<(int totalCount, List<ProjectBase> items)> Query(int tenantNumId,
                                                              string keyword,
                                                              int state,
                                                              DateTime? createStartTime,
                                                              DateTime? createEndTime,
                                                              int pageIndex,
                                                              int pageSize);

        /// <summary>
        /// 根据ID集合查询项目列表
        /// </summary>
        /// <param name="ids">ID集合</param>
        /// <returns></returns>
        Task<List<ProjectBase>> QueryByIds(List<string> ids);
    }
}
