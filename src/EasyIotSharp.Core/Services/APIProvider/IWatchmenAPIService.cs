using System.Threading.Tasks;
using EasyIotSharp.Core.Dto.Users;

namespace EasyIotSharp.Core.Services.APIProvider
{
    /// <summary>
    /// Watchmen服务
    /// </summary>
    public interface IWatchmenAPIService
    {
        /// <summary>
        ///根据用户Id从Watchmen获取用户token
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<UserTokenDto> GetUserToken(string uid);
    }
}