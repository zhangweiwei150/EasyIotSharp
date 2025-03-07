﻿using System;
using System.ComponentModel;
using UPrime.CodeAnnotations;

namespace EasyIotSharp.Core
{
    /// <summary>
    /// 数据类型
    /// -1=全部
    /// 1=string
    /// 2=int
    /// 3=double
    /// 4=float
    /// 5=bool
    /// </summary>
    public enum DataTypeMenu
    {
        /// <summary>
        /// 全部(未知的)
        /// </summary>
        [EnumAlias("全部")]
        None = -1,

        /// <summary>
        /// string
        /// </summary>
        [EnumAlias("string")]
        String = 1,

        /// <summary>
        /// int
        /// </summary>
        [EnumAlias("int")]
        Int = 2,

        /// <summary>
        /// double
        /// </summary>
        [EnumAlias("double")]
        Double = 3,

        /// <summary>
        /// float
        /// </summary>
        [EnumAlias("float")]
        Float = 4,

        /// <summary>
        /// bool
        /// </summary>
        [EnumAlias("bool")]
        Bool = 5
    }
}