using Microsoft.AspNetCore.Http;
using System.Reflection;
using UPrime.Dependency;
using UPrime.Modules;
using UPrime.SDK.Weixin;
using UPrime.SDK.Sms;
using EasyIotSharp.Core;
using UPrime.SDK.UCloudUFile;

namespace EasyIotSharp.API
{
    [DependsOn(
        typeof(EasyIotSharpCoreModule),
        typeof(UPrimeSDKWeixinModule),
        typeof(UPrimeSDKUCloudUFileModule),
        typeof(UPrimeSDKSmsModule)
       )]
    public class EasyIotSharpAPICoreModule : UPrimeModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IHttpContextAccessor, HttpContextAccessor>(DependencyLifeStyle.Singleton);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssembly(Assembly.GetExecutingAssembly());
        }
    }
}