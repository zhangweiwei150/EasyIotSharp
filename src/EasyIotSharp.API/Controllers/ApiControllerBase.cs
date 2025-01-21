using EasyIotSharp.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace EasyIotSharp.API.Controllers
{
    public class ApiControllerBase : Controller
    {
        /// <summary>
        /// 获取用户标识
        /// </summary>
        public string TokenUserId
        {
            get
            {
                var userId = HttpContext.User.Identity.GetUserTokenIdentifier()?.UserId;
                return userId.IsNotNullOrEmpty() ? userId : string.Empty;
            }
        }
    }
}