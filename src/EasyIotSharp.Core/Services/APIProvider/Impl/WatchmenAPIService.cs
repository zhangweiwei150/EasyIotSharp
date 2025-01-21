using System;
using System.Threading.Tasks;
using UPrime.WebApi;
using EasyIotSharp.Core.Configuration;
using EasyIotSharp.Core.Dto.Users;
using EasyIotSharp.Core.Dto.Watchmen.Params;

namespace EasyIotSharp.Core.Services.APIProvider.Impl
{
    public class WatchmenAPIService : ServiceBase, IWatchmenAPIService
    {
        private readonly AppOptions _appOptions;
        private readonly IUPrimeWebApiClient _apiClient;

        public WatchmenAPIService(
            AppOptions appOptions,
            IUPrimeWebApiClient apiClient)
        {
            _appOptions = appOptions;
            _apiClient = apiClient;
            _apiClient.BaseUrl = _appOptions.WatchmenOptions.APIBaseUrl;
            _apiClient.ZipkinServiceName = "service.watchmen";
        }

        public async Task<UserTokenDto> GetUserToken(string uid)
        {
            var input = new GetUserTokenInput();
            var dto = new UserTokenDto();

            input.AppId = _appOptions.WatchmenOptions.AppId;
            input.Secret = _appOptions.WatchmenOptions.Secret;
            input.GrantType = _appOptions.WatchmenOptions.GrantType;
            input.UId = uid;

            var res = await _apiClient.PostAsync<GetUserTokenOutput>(
               "/wm.app.token", input);

            if (res.Code == 1)
            {
                dto.AccessToken = res.AccessToken;
                dto.ExpirationTime = DateTime.Now.AddSeconds(res.ExpiredIn);
            }
            return dto;
        }
    }
}