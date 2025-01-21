using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyIotSharp.Core.Validator
{
    public class IsEmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string val = value as string;
            if (val.IsNotNullOrWhiteSpace())
            {
                if (!Regex.IsMatch(val, RegexExpressions.EMAIL))
                {
                    ErrorMessage = "邮箱格式不正确!";
                    return false;
                }
            }
            return true;
        }
    }
}