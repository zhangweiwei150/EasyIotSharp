using System.Text.RegularExpressions;

namespace EasyIotSharp.Core.Extensions
{
    public static class MatchExtensions
    {
        #region 手机号码的正确性

        /// <summary>
        /// 匹配手机号码
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <param name="isMatch">是否匹配成功，若返回true，则会得到一个Match对象，否则为null</param>
        /// <returns>匹配对象</returns>
        public static Match MatchPhoneNumber(this string s, out bool isMatch)
        {
            if (string.IsNullOrEmpty(s))
            {
                isMatch = false;
                return null;
            }
            Match match = Regex.Match(s, @"^((1[3,5,6,8][0-9])|(14[5,7])|(17[0,1,3,6,7,8])|(19[5,8,9]))\d{8}$");
            isMatch = match.Success;
            return isMatch ? match : null;
        }

        /// <summary>
        /// 匹配手机号码
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <returns>是否匹配成功</returns>
        public static bool MatchPhoneNumber(this string s)
        {
            MatchPhoneNumber(s, out bool success);
            return success;
        }

        #endregion 手机号码的正确性
    }
}