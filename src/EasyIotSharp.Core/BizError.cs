namespace EasyIotSharp.Core
{
    /// <summary>
    /// 定义业务的全局错误返回
    /// </summary>
    public class BizError
    {
        public static BizError UNKNOWN_ERROR = new BizError(-1, "未知错误");
        public static BizError NO_HANDLER_FOUND = new BizError(10001, "未找到指定的资源");
        public static BizError BIND_EXCEPTION_ERROR = new BizError(10002, "请求参数错误");
        public static BizError PARAMTER_VALIDATION_ERROR = new BizError(10003, "请求参数校验失败");
        public static BizError ELASTICSEARCH_ERROR = new BizError(10004, "elasticsearch出错了");
        public static BizError OBJECT_CONVERT_ERROR = new BizError(10005, "对象类型转换失败");
        public static BizError SIGN_VERIFY_ERROR = new BizError(10006, "签名失败");

        // 业务错误从20000开始

        //JWT
        public static BizError TOKEN_EXPIRED = new BizError(40003, "token expired");
        public static BizError TOKEN_EXCEPTION = new BizError(40004, "token exception");
        public static BizError TOKEN_NULLOREMPTY = new BizError(40005, "token nullorempty");
        public static BizError TOKEN_SIGNATURE = new BizError(40006, "token signerror");
        public static BizError TOKEN_INVALID_USER_CLAIMS= new BizError(40006, "token claimserror");

        public static BizError USER_MOBILE_VERIFY_ERROR = new BizError(20004, "验证码错误");
        public static BizError USER_MOBILE_VERIFY_EXPIRED = new BizError(20005, "验证码超时");


        #region Bytedance

        public static BizError BYTEDANCE_ACCESSTCOKEN_ERROR = new BizError(20000, "获取AccessToken异常");
        public static BizError BYTEDANCE_TEXT_VALIDATE_ERROR = new BizError(20001, "文本验证失败");

        #endregion Bytedance

        public BizError(int errCode, string errMessage)
        {
            ErrCode = errCode;
            ErrMessage = errMessage;
        }

        public BizError(BizError err)
        {
            this.ErrCode = err.ErrCode;
            this.ErrMessage = err.ErrMessage;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrCode { get; private set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMessage { get; private set; }
    }
}