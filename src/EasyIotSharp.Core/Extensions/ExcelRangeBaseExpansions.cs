using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyIotSharp.Core.Extensions
{
    /// <summary>
    /// 导出Excel拓展
    /// </summary>
    public static class ExcelRangeBaseExtensions
    {
        /// <summary>
        /// 从IEnumerable加载数据拓展(定制忽略属性)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="collection"></param>
        /// <param name="ignorePropertyNames"></param>
        /// <param name="printHeaders"></param>
        /// <returns></returns>
        public static ExcelRangeBase LoadFromCollection<T>(this ExcelRangeBase @this, IEnumerable<T> collection, string[] ignorePropertyNames, bool printHeaders) where T : class
        {
            MemberInfo[] membersToInclude = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => ignorePropertyNames.Contains(p.Name) == false).ToArray();
            return @this.LoadFromCollection<T>(collection, printHeaders, OfficeOpenXml.Table.TableStyles.None, BindingFlags.Instance | BindingFlags.Public, membersToInclude);
        }

        /// <summary>
        /// 根据数值获取excel对应的列名（address letter）
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string GetExcelColumnName(this int col)
        {
            if (col <= 26)
            {
                return Convert.ToChar(col + 64).ToString();
            }
            var div = col / 26;
            var mod = col % 26;
            if (mod != 0) return GetExcelColumnName(div) + GetExcelColumnName(mod);
            mod = 26; div--;
            return GetExcelColumnName(div) + GetExcelColumnName(mod);
        }
    }
}