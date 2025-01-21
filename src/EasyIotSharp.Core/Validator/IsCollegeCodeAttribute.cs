using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EasyIotSharp.Core.Validator
{
    /// <summary>
    /// 院校代码验证
    /// </summary>
    public class IsCollegeCodeAttribute : ValidationAttribute
    {
        /// <summary>
        /// 验证院校代码格式及长度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var val = value.ToString();
            if (val.IsNullOrEmpty())
            {
                ErrorMessage = "院校代码不能为空";
                return false;
            }
            if (!Regex.IsMatch(val, RegexExpressions.COLLEGE_CODE))
            {
                ErrorMessage = "院校代码格式必须为五位数字";
                return false;
            }
            return true;
        }
    }
}