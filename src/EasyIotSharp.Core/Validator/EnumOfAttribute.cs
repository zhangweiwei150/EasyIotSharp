using System;
using System.ComponentModel.DataAnnotations;

namespace EasyIotSharp.Core.Validator
{
    /// <summary>
    /// 判断枚举是否存在
    /// </summary>
    public class EnumOfAttribute : ValidationAttribute
    {
        private Type Type { get; set; }

        public EnumOfAttribute(Type value)
        {
            Type = value;
        }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            return Enum.IsDefined(Type, value);
        }
    }
}