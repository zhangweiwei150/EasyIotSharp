using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Project.Params
{
    /// <summary>
    /// 通过条件分页查询项目分类列表的入参类
    /// </summary>
    public class QueryClassificationInput:PagingInput
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 是否分页
        /// true=分页
        /// false=查询全部
        /// </summary>
        public bool IsPage { get; set; }
    }
}
