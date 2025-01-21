using System;
using System.Collections.Generic;
using System.Linq;
using UPrime;
using EasyIotSharp.Core.Dto.Common;

namespace EasyIotSharp.Core.Services.Common.Impl
{
    public class ProvinceService : IProvinceService
    {
        #region Ctor

        private static readonly Lazy<List<ProvinceDto>> LazyProvince = new Lazy<List<ProvinceDto>>(InitializeProvince);

        /// <summary>
        /// 省份
        /// </summary>
        public static List<ProvinceDto> ProvinceList => LazyProvince.Value;

        #endregion Ctor

        #region Province Data

        private static List<ProvinceDto> InitializeProvince()
        {
            return new List<ProvinceDto>
            {
                new ProvinceDto() {Code = "11", Name = "北京", FriendlyName = "北京市", Letter = "B",IsNewGaoKao=true,NumId=834},
                new ProvinceDto() {Code = "12", Name = "天津", FriendlyName = "天津市", Letter = "T",IsNewGaoKao=true,NumId=835},
                new ProvinceDto() {Code = "13", Name = "河北", FriendlyName = "河北省", Letter = "H",IsNewGaoKao=true,NumId=1128},
                new ProvinceDto() {Code = "14", Name = "山西", FriendlyName = "山西省", Letter = "S",NumId=837},
                new ProvinceDto() {Code = "15", Name = "内蒙古", FriendlyName = "内蒙古自治区", Letter = "N",NumId=838},
                new ProvinceDto() {Code = "21", Name = "辽宁", FriendlyName = "辽宁省", Letter = "L",IsNewGaoKao=true,NumId=839},
                new ProvinceDto() {Code = "22", Name = "吉林", FriendlyName = "吉林省", Letter = "J",NumId=840},
                new ProvinceDto() {Code = "23", Name = "黑龙江", FriendlyName = "黑龙江省", Letter = "H",NumId=841},
                new ProvinceDto() {Code = "31", Name = "上海", FriendlyName = "上海市", Letter = "S",IsNewGaoKao=true,NumId=842},
                new ProvinceDto() {Code = "32", Name = "江苏", FriendlyName = "江苏省", Letter = "J",IsNewGaoKao=true,NumId=1},
                new ProvinceDto() {Code = "33", Name = "浙江", FriendlyName = "浙江省", Letter = "Z",IsNewGaoKao=true,NumId=843},
                new ProvinceDto() {Code = "34", Name = "安徽", FriendlyName = "安徽省", Letter = "A",NumId=844},
                new ProvinceDto() {Code = "35", Name = "福建", FriendlyName = "福建省", Letter = "F",IsNewGaoKao=true,NumId=845},
                new ProvinceDto() {Code = "36", Name = "江西", FriendlyName = "江西省", Letter = "J",NumId=846},
                new ProvinceDto() {Code = "37", Name = "山东", FriendlyName = "山东省", Letter = "S",IsNewGaoKao=true,NumId=847},
                new ProvinceDto() {Code = "41", Name = "河南", FriendlyName = "河南省", Letter = "H",NumId=848},
                new ProvinceDto() {Code = "42", Name = "湖北", FriendlyName = "湖北省", Letter = "H",IsNewGaoKao=true,NumId=849},
                new ProvinceDto() {Code = "43", Name = "湖南", FriendlyName = "湖南省", Letter = "H",IsNewGaoKao=true,NumId=850},
                new ProvinceDto() {Code = "44", Name = "广东", FriendlyName = "广东省", Letter = "G",IsNewGaoKao=true,NumId=851},
                new ProvinceDto() {Code = "45", Name = "广西", FriendlyName = "广西壮族自治区", Letter = "G",NumId=852},
                new ProvinceDto() {Code = "46", Name = "海南", FriendlyName = "海南省", Letter = "H",IsNewGaoKao=true,NumId=853},
                new ProvinceDto() {Code = "50", Name = "重庆", FriendlyName = "重庆市", Letter = "C",IsNewGaoKao=true,NumId=854},
                new ProvinceDto() {Code = "51", Name = "四川", FriendlyName = "四川省", Letter = "S",NumId=855},
                new ProvinceDto() {Code = "52", Name = "贵州", FriendlyName = "贵州省", Letter = "G",NumId=856},
                new ProvinceDto() {Code = "53", Name = "云南", FriendlyName = "云南省", Letter = "Y",NumId=857},
                new ProvinceDto() {Code = "54", Name = "西藏", FriendlyName = "西藏自治区", Letter = "X",NumId=858},
                new ProvinceDto() {Code = "61", Name = "陕西", FriendlyName = "陕西省", Letter = "S",NumId=859},
                new ProvinceDto() {Code = "62", Name = "甘肃", FriendlyName = "甘肃省", Letter = "G",NumId=860},
                new ProvinceDto() {Code = "63", Name = "青海", FriendlyName = "青海省", Letter = "Q",NumId=861},
                new ProvinceDto() {Code = "64", Name = "宁夏", FriendlyName = "宁夏回族自治区", Letter = "N",NumId=862},
                new ProvinceDto() {Code = "65", Name = "新疆", FriendlyName = "新疆维吾尔自治区", Letter = "X",NumId=1120},
                new ProvinceDto() {Code = "71", Name = "台湾", FriendlyName = "台湾省", Letter = "T",NumId=-1},
                new ProvinceDto() {Code = "81", Name = "香港", FriendlyName = "香港特别行政区", Letter = "X",NumId=16733},
                new ProvinceDto() {Code = "82", Name = "澳门", FriendlyName = "澳门特别行政区", Letter = "A",NumId=19340}
            };
        }

        #endregion Province Data

        #region GET / IS / SEARCH

        /// <summary>
        /// 通过代号获取省份信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ProvinceDto GetByCode(string code)
        {
            var result = ProvinceList.FirstOrDefault(x => x.Code == code);
            return result;
        }

        /// <summary>
        /// 通过代号获取省份信息
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public List<ProvinceDto> GetByCodes(IList<string> codes)
        {
            var result = ProvinceList.Where(x => codes.Contains(x.Code));
            return result.ToList();
        }

        /// <summary>
        /// 通过代号获取省份信息
        /// </summary>
        /// <param name="numId"></param>
        /// <returns></returns>
        public ProvinceDto GetByNunId(int numId)
        {
            var result = ProvinceList.FirstOrDefault(x => x.NumId == numId);
            return result;
        }

        /// <summary>
        /// 通过省份名称获取省份信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ProvinceDto GetByName(string name)
        {
            return ProvinceList.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// 获取所有省份列表（静态的）
        /// </summary>
        /// <returns></returns>
        public List<ProvinceDto> QueryAll()
        {
            return ProvinceList;
        }

        #endregion GET / IS / SEARCH
    }
}