using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EasyIotSharp.Core.Validator
{
    /// <summary>
    /// 字符串数组项长度和数组大小验证
    /// </summary>
    public class StringItemsLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// 字符串数组项长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 数组大小
        /// </summary>
        public int MinSize { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="length">字符串数组项长度</param>
        /// <param name="minSize">数组大小</param>
        public StringItemsLengthAttribute(int length, int minSize = 0)
        {
            Length = length;
            MinSize = minSize;
        }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return false;
            }
            var isMatch = true;
            var list = value as IList<string>;
            if (list.Count < MinSize)
            {
                ErrorMessage = $"数组长度必须大于{MinSize}";
                return false;
            }
            if (list.Any())
            {
                var isExist = list.Any(x => x.Length != Length);
                if (isExist)
                {
                    ErrorMessage = $"数组中存在不符合长度的值,值的长度为{Length}";
                    isMatch = false;
                }
            }
            return isMatch;
        }
    }
}