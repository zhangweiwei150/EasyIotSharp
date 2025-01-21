using Microsoft.AspNetCore.Http;
using System.Reflection;
using UPrime.Dependency;
using UPrime.Modules;
using UPrime.SDK.Sms;
using UPrime.SDK.UCloudUFile;
using UPrime.SDK.Weixin;
using EasyIotSharp.Core;

namespace EasyIotSharp.API.Admin
{
    [DependsOn(
        typeof(EasyIotSharpCoreModule),
        typeof(UPrimeSDKUCloudUFileModule),
        typeof(UPrimeSDKWeixinModule),
        typeof(UPrimeSDKSmsModule)
       )]
    public class EasyIotSharpAdminCoreModule : UPrimeModule
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