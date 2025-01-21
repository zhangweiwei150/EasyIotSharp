using System.ComponentModel.DataAnnotations;

namespace EasyIotSharp.Core.Dto
{
    /// <summary>
    /// 操作用户信息输入
    /// </summary>
    public class OperateUserInput
    {
        /// <summary>
        /// 操作人标识
        /// </summary>
        [Required]
        public string OperatorId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [Required]
        public string OperatorName { get; set; }
    }
}