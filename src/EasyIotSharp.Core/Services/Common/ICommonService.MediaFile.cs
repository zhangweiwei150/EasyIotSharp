using UPrime;
using EasyIotSharp.Core.Dto.Common.Params;

namespace EasyIotSharp.Core.Services.Common
{
    /// <summary>
    /// UFile服务
    /// </summary>
    public partial interface ICommonService
    {
        /// <summary>
        /// 图片文件上传
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ActionOutput<string> UploadImage(UploadImageInput input);
    }
}