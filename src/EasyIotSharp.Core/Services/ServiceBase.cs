using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UPrime;
using UPrime.Events.Bus;
using UPrime.Reflection;
using UPrime.Services;
using EasyIotSharp.Core.Configuration;
using EasyIotSharp.Core.Extensions;

/// <summary>
/// 所有应用服务的基类，实现类必须继承此类
/// </summary>
public abstract class ServiceBase : ApplicationServiceBase
{
    public EventBus EventBus => EventBus.Default;
    public ITypeFinder TypeFinder;
    public IHttpContextAccessor _httpContextAccessor;

    public ServiceBase()
    {
        TypeFinder = UPrimeEngine.Instance.Resolve<ITypeFinder>();
        try
        {
            _httpContextAccessor = UPrimeEngine.Instance.Resolve<IHttpContextAccessor>();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 获取当前上下文用户对象
    /// </summary>
    public UserTokenData ContextUser
    {
        get
        {
            return _httpContextAccessor.HttpContext.User.Identity.GetUserTokenIdentifier();
        }
    }

    /// <summary>
    /// 空返回相当于void
    /// </summary>
    /// <returns></returns>
    public Task TaskReturnNull()
    {
        return Task.FromResult(0);
    }
}