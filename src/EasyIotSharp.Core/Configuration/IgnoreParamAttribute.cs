using System;

namespace EasyIotSharp.Core.Configuration
{
    /// <summary>
    /// 实体转换为字典时忽略的参数
    /// </summary>
    public class IgnoreParamAttribute : Attribute
    {
        public string PropName { get; set; }

        public IgnoreParamAttribute()
        {
        }

        public IgnoreParamAttribute(string propName, Action action)
        {
            PropName = propName;
        }
    }
}