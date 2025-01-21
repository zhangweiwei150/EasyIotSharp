using System;
using System.Security.Cryptography;
using System.Text;

namespace EasyIotSharp.Core.Extensions
{
    /// <summary>
    /// 加密扩展
    /// </summary>
    public static class EncryptExtensions
    {
        #region SHA1加密

        public static string EncryptSHA1(string encryptStr)
        {
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(encryptStr);//获得以UTF8编码格式的子节数组
            byte[] resultArray = new SHA1CryptoServiceProvider().ComputeHash(toEncryptArray);
            return BitConverter.ToString(resultArray).Replace("-", "").ToLower();
        }

        #endregion SHA1加密

        #region 时间戳

        /// <summary>
        /// 获取时间戳（把当前时间转换为毫秒数）
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        #endregion 时间戳
    }
}