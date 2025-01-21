using System;
using UPrime.WebApi;

namespace EasyIotSharp.Core
{
    /// <summary>
    /// 通用的业务错误异常包装
    /// </summary>
    public class BizException : Exception
    {
        public UPrimeResponse CommonError { get; private set; }

        public BizException(BizError bizError)
        {
            CommonError = new UPrimeResponse();
            CommonError.SetMessage(bizError.ErrCode.ToString(), bizError.ErrMessage);
        }

        public BizException(BizError bizError, string errMsg)
        {
            CommonError = new UPrimeResponse();
            CommonError.SetMessage(bizError.ErrCode.ToString(), errMsg);
        }
    }
}