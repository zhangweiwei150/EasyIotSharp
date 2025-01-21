using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Domain.TenantAccount
{
    /// <summary>
    /// 租户用户表
    /// </summary>
    [SugarTable("Soldiers")]
    public class Soldier : BaseEntity<int>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 是否管理员
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
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否测试
        /// </summary>
        public bool IsTest { get; set; }

        /// <summary>
        /// 照片Id
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// 照片路径
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// 姓别：1=男，2=女，-1 = 不选
        /// </summary>
        public int Sex { get; set; } = -1;

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }

        /// <summary>
        /// 身份证照路径
        /// </summary>
        public string IdCardPicUrl { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpiredTime { get; set; }
    }
}
