using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EasyIotSharp.Core.Validator
{
    /// <summary>
    /// 省份代码格式验证
    /// </summary>
    public class IsProvinceCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null)
            {
                ErrorMessage = "值不能为空";
                return false;
            }
            var val = value.ToString();
            if (!Regex.IsMatch(val, RegexExpressions.PROVINCE_CODE))
            {
                ErrorMessage = "省份代码格式不正确";
                return false;
            }
            return true;
        }
    }
}