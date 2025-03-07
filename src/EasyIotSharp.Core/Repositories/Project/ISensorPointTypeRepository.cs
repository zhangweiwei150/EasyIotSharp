﻿using EasyIotSharp.Core.Domain.Proejct;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.Project
{
    public interface ISensorPointTypeRepository : IMySqlRepositoryBase<SensorPointType, string>
    {
        /// <summary>
        /// 通过条件分页查询测点类型列表
        /// </summary>
        /// <param name="tenantNumId">租户NumId</param>
        /// <param name="keyword">名称/简称/厂家</param>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页多少条数据</param>
        /// <param name="isPage">是否分页</param>
        /// <returns></returns>
        Task<(int totalCount, List<SensorPointType> items)> Query(int tenantNumId,
                                                                  string keyword,
                                                                  int pageIndex,
                                                                  int pageSize,
                                                                  bool isPage);
    }
}
