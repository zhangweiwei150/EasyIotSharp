using System.Collections.Generic;
using UPrime;
using UPrime.Dependency;
using EasyIotSharp.Core.Dto.Common;

namespace EasyIotSharp.Core.Services.Common
{
    /// <summary>
    /// 地区的省份服务，提供静态的省份的处理业务
    /// </summary>
    public interface IProvinceService : ISingletonDependency
    {
        /// <summary>
        /// 通过国标获取省份
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ProvinceDto GetByCode(string code);

        /// <summary>
        ///
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        List<ProvinceDto> GetByCodes(IList<string> codes);

        /// <summary>
        /// 通过省份名称获取省份信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ProvinceDto GetByName(string name);

        /// <summary>
        /// 获取所有省份列表（静态的）
        /// </summary>
        /// <returns></returns>
        List<ProvinceDto> QueryAll();
    }
}