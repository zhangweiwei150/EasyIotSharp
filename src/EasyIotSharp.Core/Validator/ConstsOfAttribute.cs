using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EasyIotSharp.Core.Validator
{
    /// <summary>
    /// 常量验证
    /// </summary>
    public class ConstsOfAttribute : ValidationAttribute
    {
        /// <summary>
        /// 类型
        /// </summary>
        private Type Type { get; set; }

        public ConstsOfAttribute(Type value)
        {
            Type = value;
        }
        /// <summary>
        /// 验证常量值是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var fields = Type.GetFields();
            var isExistValue = fields.Any(x => x.GetRawConstantValue()?.ToString() == value?.ToString());
            if (!isExistValue)
            {
                ErrorMessage = $"参数可选值范围错误，应为:{fields.Select(s => s.GetRawConstantValue()).JoinAsString(",")}";
                return false;
            }
            return isExistValue;
        }
    }
}