using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UPrime;
using UPrime.Utils.Words;
using EasyIotSharp.Core.Configuration;
using EasyIotSharp.Core.Services.Common;
using static EasyIotSharp.Core.GlobalConsts;

namespace EasyIotSharp.Core.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.bool bflag = true</param>
        /// <returns></returns>
        public static string GetTimeStamp(this bool bflag)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

            return ret;
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetTimeStamp(this DateTime dateTime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long unixTime = (long)Math.Round((dateTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }

        /// <summary>
        /// Unix时间戳转DateTime
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(this string timestamp)
        {
            DateTime time = DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            if (timestamp.Length == 10)        //精确到秒
            {
                time = startTime.AddSeconds(double.Parse(timestamp));
            }
            else if (timestamp.Length == 13)   //精确到毫秒
            {
                time = startTime.AddMilliseconds(double.Parse(timestamp));
            }
            else
            {
                time = DateTime.Now;
            }
            return time;
        }

        /// <summary>
        /// 实体转换为字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ConvertToDic<T>(this T t)
        {
            var dics = new Dictionary<string, string>();
            var type = t.GetType();
            var members = type.GetRuntimeProperties();
            foreach (var member in members)
            {
                if (member.IsNull())
                    continue;
                var isIgnore = member.GetCustomAttributesData().Any(x => x.AttributeType.Name == typeof(IgnoreParamAttribute).Name);
                if (member.PropertyType.IsPublic && !isIgnore)
                {
                    var value = member.GetValue(t);
                    var val = Convert.ToString(value);
                    if (val.IsNotNullOrWhiteSpace())
                    {
                        dics.Add(member.Name.Substring(0, 1).ToLower() + member.Name.Substring(1), val);
                    }
                }
            }
            return dics;
        }

        /// <summary>
        /// 加密11位手机号
        /// </summary>
        /// <param name="mobileNumber">手机号</param>
        /// <returns></returns>
        public static string EncryptMobileNumber(this string mobileNumber)
        {
            if (string.IsNullOrEmpty(mobileNumber))
                return mobileNumber;
            if (mobileNumber.Length != 11)
                return mobileNumber;
            string result = string.Empty;
            result = Regex.Replace(mobileNumber, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
            return result;
        }

        /// <summary>
        /// String to MongoDb.Bson.ObjectId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ObjectId ToObjectId(this string id)
        {
            return new ObjectId(id);
        }

        /// <summary>
        /// 在0到10直接随机生成几个数字
        /// </summary>
        /// <param name="num">控制几位数字</param>
        /// <returns></returns>
        public static string GenerateRadomPassword(this int num)
        {
            Random rand = new Random();
            string pass = "";
            for (int i = 0; i < num; i++)
            {
                pass += rand.Next(0, 10).ToString();
            }
            return pass;
        }

        /// <summary>
        /// 生成随机的手机验证码
        /// </summary>
        /// <returns></returns>
        public static string GenerateMobileAuthCode(this int length)
        {
            string characterSet = "0123456789";
            Random random = new Random();
            string randomCode = new string(
                Enumerable.Repeat(characterSet, length)
                    .Select(set => set[random.Next(set.Length)])
                    .ToArray());
            return randomCode;
        }

        /// <summary>
        /// 对象添加属性
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <param name="value">属性名称使用类名</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static ExpandoObject AddProperty(this object sourceObj, object value = null)
        {
            IDictionary<string, object> result = new ExpandoObject();
            var defaultName = value.GetType().Name;
            foreach (PropertyDescriptor pro in TypeDescriptor.GetProperties(sourceObj.GetType()))
            {
                result.Add(pro.Name, pro.GetValue(sourceObj));
            }
            if (result.ContainsKey(defaultName))
            {
                throw new Exception("对象已存在该属性！");
            }

            result.TryAdd(defaultName, value);
            return result as ExpandoObject;
        }

        /// <summary>
        /// 对象添加属性
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <param name="name">指定的属性名称</param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static ExpandoObject AddProperty(this object sourceObj, string name = "", object value = null)
        {
            IDictionary<string, object> result = new ExpandoObject();
            var defaultName = value.GetType().Name;
            foreach (PropertyDescriptor pro in TypeDescriptor.GetProperties(sourceObj.GetType()))
            {
                result.Add(pro.Name, pro.GetValue(sourceObj));
            }
            if (result.ContainsKey(name))
            {
                throw new Exception("对象已存在该属性！");
            }
            if (name.IsNullOrWhiteSpace())
                name = defaultName;

            result.TryAdd(name, value);
            return result as ExpandoObject;
        }

        /// <summary>
        /// 移除指定的属性名称
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <param name="name">属性名称使用类名</param>
        /// <returns></returns>
        public static ExpandoObject RemoveProperty(this object sourceObj, string name)
        {
            IDictionary<string, object> result = new ExpandoObject();
            foreach (PropertyDescriptor pro in TypeDescriptor.GetProperties(sourceObj.GetType()))
            {
                result.Add(pro.Name, pro.GetValue(sourceObj));
            }
            if (result.ContainsKey(name))
            {
                result.Remove(name);
            }
            return result as ExpandoObject;
        }
    }
}