using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 通过项目id删除一条项目信息的入参类
    /// </summary>
    public class DeleteProjectBaseInput
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
