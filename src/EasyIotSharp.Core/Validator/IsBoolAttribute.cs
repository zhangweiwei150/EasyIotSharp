using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EasyIotSharp.Core.Validator
{
    /// <summary>
    /// Bool类型验证
    /// </summary>
    public class IsBoolAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null)
            {
                ErrorMessage = "值不能为空";
                return false;
            }
            var val = value.ToString();
            if (!Regex.IsMatch(val, RegexExpressions.BOOL_TRUE_FALSE))
            {
                ErrorMessage = "格式不正确：正确格式值true和false";
                return false;
            }
            return true;
        }
    }
}