using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.TenantAccount.Params
{
    /// <summary>
    /// 添加一条用户信息的入参类
    /// </summary>
    public class InsertSoldierInput:OperateUserInput
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantNumId { get; set; }

        /// <summary>
        /// 是否admin(超级管理员，没有租户限制)
        /// </summary>
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// 是否管理员
        /// 1=管理员
        /// 2=普通用户
        /// </summary>
        public int IsManager { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 是否测试
        /// </summary>
        public bool IsTest { get; set; }

        /// <summary>
        /// 姓别：1=男，2=女，-1 = 不选
        /// </summary>
        public int Sex { get; set; } = -1;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
    }
}
