using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 通过id删除一条项目分类的入参类
    /// </summary>
    public class DeleteClassificationInput
    {
        /// <summary>
        /// 项目分类id
        /// </summary>
        public string Id { get; set; }
    }
}
