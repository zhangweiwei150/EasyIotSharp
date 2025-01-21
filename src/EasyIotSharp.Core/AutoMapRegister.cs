using AutoMapper;
using System;
using UPrime;
using UPrime.AutoMapper;
using EasyIotSharp.Core.Extensions;
using static EasyIotSharp.Core.GlobalConsts;

namespace EasyIotSharp.Core
{
    public class AutoMapRegister : IAutoMapRegistrar
    {
        public void RegisterMaps(IMapperConfigurationExpression config)
        {
            //config.CreateMap<PolyvWatchLog_ES, ExportPolyvWatchLogDataDto>()
                //.ForMember(dto => dto.UserMobile, opt => opt.MapFrom(f => f.UserMobile.EncryptMobileNumber()));

        }
    }
}