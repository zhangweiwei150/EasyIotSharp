using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 通过id修改一条项目分类的入参类
    /// </summary>
    public class UpdateClassificationInput
    {
        /// <summary>
        /// 项目分类id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序字段
        /// 字段越大越靠前
        /// </summary>
        public int Sort { get; set; }
    }
}
