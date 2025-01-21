using System.ComponentModel.DataAnnotations;

namespace EasyIotSharp.Core.Dto
{
    /// <summary>
    /// 删除输入参数
    /// </summary>
    public class DeleteAdminInput : OperateUserInput
    {
        /// <summary>
        /// 对象id
        /// </summary>
        [RegularExpression(RegexExpressions.OBJECT_ID)]
        public string Id { get; set; }
    }
}