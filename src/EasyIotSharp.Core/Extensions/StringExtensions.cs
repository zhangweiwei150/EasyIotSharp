using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyIotSharp.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        ///  去掉字符串中的特殊字符
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string ReplaceCharsByEmpty(this string chars)
        {
            if (chars.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }
            chars = chars.RemoveHtml();
            //用正则去掉所有标点符号
            chars = Regex.Replace(chars, "[\\s\\p{P}\n\r=<>$>+￥^]", "");
            chars = Regex.Replace(chars, "[ \\[ \\] \\^ \\-_*×――(^)（^）$%~!@#$…&%￥—+=<>《》【】!！??？:：•`·、。，；,.;\"‘’“”-]", "");
            //去掉字母
            chars = Regex.Replace(chars, "[a-z]", "", RegexOptions.IgnoreCase);
            //去掉数字
            chars = Regex.Replace(chars, "[0-9]", "", RegexOptions.IgnoreCase);
            //去掉转义字符
            chars = Regex.Replace(chars, @"'", "");
            chars = Regex.Replace(chars, @"\\", "");
            chars = Regex.Replace(chars, @"\b", "");
            chars = Regex.Replace(chars, @"\n", "");
            chars = Regex.Replace(chars, @"\r\n", "");
            chars = Regex.Replace(chars, @"\t", "");
            chars = Regex.Replace(chars, @"\v", "");
            chars = Regex.Replace(chars, @"\f", "");
            chars = chars.Trim();
            return chars;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string ConvertEncodeBase64(this string source)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(source));
        }

        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5(this string input)
        {
            var md5 = MD5.Create();//实例化一个md5对像
            var s = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return s.Aggregate("", (current, t) => current + t.ToString("X2"));
        }

        /// <summary>
        /// 转换格式
        /// </summary>
        /// <param name="unicodeString"></param>
        /// <returns></returns>
        public static string GetUtf8(this string unicodeString)
        {
            var utf8 = new UTF8Encoding();
            var encodedBytes = utf8.GetBytes(unicodeString);
            var decodedString = utf8.GetString(encodedBytes);
            return decodedString;
        }

        /// <summary>
        /// 是否为字母
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsLetter(this string content, string pattern = @"^[a-zA-Z]+$") => Regex.IsMatch(content, pattern);

        /// <summary>
        /// 长链接转短链接，用于发送带链接的短信，邮件等
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetShortUrl(this string url)
        {
            string resUrl = string.Empty;

            //可以自定义生成MD5加密字符传前的混合KEY
            string key = "012345678.+*-=";
            //要使用生成URL的字符
            string[] chars = new string[]
            {
                 "a", "b", "c", "d", "e", "f", "g", "h",
                 "i", "j", "k", "l", "m", "n", "o", "p",
                 "q", "r", "s", "t", "u", "v", "w", "x",
                 "y", "z", "0", "1", "2", "3", "4", "5",
                "6", "7", "8", "9", "A", "B", "C", "D",
              "E", "F", "G", "H", "I", "J", "K", "L",
               "M", "N", "O", "P", "Q", "R", "S", "T",
                "U", "V", "W", "X", "Y", "Z"
           };
            //对传入网址进行MD5加密
            string hex = (key + url).Md5();
            //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
            int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(1 * 8, 8), 16);
            string outChars = string.Empty;
            for (int j = 0; j < 6; j++)
            {
                //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                int index = 0x0000003D & hexint;
                //把取得的字符相加
                outChars += chars[index];
                //每次循环按位右移5位
                hexint >>= 5;
            }
            //把字符串存入对应索引的输出数组
            resUrl = outChars;
            return resUrl;
        }

        /// <summary>
        /// 手机号脱敏
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string DesensitizationTreatment(this string num)
        {
            if (string.IsNullOrEmpty(num)) return num;
            string result = "";
            if (num.Length > 5)
            {
                string pattern = @"(?<=[\w]{3})\d(?=[\S]{4})";
                result = Regex.Replace(num, pattern, "*");
                return result;
            }
            return num;
        }
    }
}