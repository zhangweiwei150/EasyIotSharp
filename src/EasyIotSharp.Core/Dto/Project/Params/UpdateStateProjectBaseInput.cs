using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 通过id修改一条项目信息的状态的入参类
    /// </summary>
    public class UpdateStateProjectBaseInput
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public int State { get; set; }
    }
}
