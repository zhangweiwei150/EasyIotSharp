using UPrime.Services.Dto;

namespace EasyIotSharp.Core.Dto.Common
{
    /// <summary>
    /// 静态省份数据dto
    /// </summary>
    public class ProvinceDto : IDto
    {
        /// <summary>
        /// 省份国标
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 友好名称（带省市）
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        public string Letter { get; set; }

        /// <summary>
        /// 是否为新高考
        /// </summary>
        public bool IsNewGaoKao { get; set; }

        /// <summary>
        /// 老的省份id
        /// <remarks>
        /// 老的省份id,仅供过渡使用,所有新业务请使用code
        /// </remarks>
        /// </summary>
        public int NumId { get; set; }
    }
}