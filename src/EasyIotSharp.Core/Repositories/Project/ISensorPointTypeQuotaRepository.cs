﻿using EasyIotSharp.Core.Domain.Proejct;
using EasyIotSharp.Core.Repositories.Mysql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Repositories.Project
{
    public interface ISensorPointTypeQuotaRepository : IMySqlRepositoryBase<SensorPointTypeQuota, string>
    {
        /// <summary>
        /// 通过条件分页查询传感器指标列表
        /// </summary>
        /// <param name="tenantNumId">租户NumId</param>
        /// <param name="sensorPointTypeId">传感器类型id</param>
        /// <param name="keyword">名称/标识符</param>
        /// <param name="dataType">数据类型  -1=全部 1=string 2=int 3=double 4=float 5=bool </param>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页多少条数据</param>
        /// <param name="isPage">是否分页</param>
        /// <returns></returns>
        Task<(int totalCount, List<SensorPointTypeQuota> items)> Query(int tenantNumId,
                                                                  string sensorPointTypeId,
                                                                  string keyword,
                                                                  DataTypeMenu dataType,
                                                                  int pageIndex,
                                                                  int pageSize,
                                                                  bool isPage);
    }
}
