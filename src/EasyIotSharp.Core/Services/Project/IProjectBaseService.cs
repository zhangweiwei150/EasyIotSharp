﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyIotSharp.Core.Dto.Project;
using UPrime.Services.Dto;
using EasyIotSharp.Core.Dto.Project.Params;

namespace EasyIotSharp.Core.Services.Project
{
    public interface IProjectBaseService
    {
        /// <summary>
        /// 通过id获取一条项目信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProjectBaseDto> GetProjectBase(string id);

        /// <summary>
        /// 通过条件分页查询项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProjectBaseDto>> QueryProjectBase(QueryProjectBaseInput input);

        /// <summary>
        /// 添加一条项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task InsertProjectBase(InsertProjectBaseInput input);

        /// <summary>
        /// 通过id修改一条项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateProjectBase(UpdateProjectBaseInput input);

        /// <summary>
        /// 通过id修改一条项目信息的状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateStateProjectBase(UpdateStateProjectBaseInput input);

        /// <summary>
        /// 通过id删除一条项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteProjectBase(DeleteProjectBaseInput input);
    }
}
