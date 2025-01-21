using System;
using System.IO;
using UPrime;
using EasyIotSharp.Core.Dto.Common.Params;
using EasyIotSharp.Core.Extensions;

namespace EasyIotSharp.Core.Services.Common.Impl
{
    public partial class CommonService
    {
        public ActionOutput<string> UploadImage(UploadImageInput input)
        {
            var res = new ActionOutput<string>();
            var fileExt = Path.GetExtension(input.ImgFile.FileName);

            #region 文件验证

            if (fileExt.ToLower() != ".jpg" && fileExt.ToLower() != ".png" && fileExt.ToLower() != ".gif")
            {
                res.AddError("文件名必须以.jpg、.png、，gif结束");
                return res;
            }
            //不去掉空格，前台显示不了
            var fileName = input.ImgFile.FileName.Replace(" ", "");
            fileName = $"youz/media/files/{DateTime.Now.ToString("yyMMdd")}_{Guid.NewGuid().ToString().ReplaceByEmpty("-")}/" + fileName;

            #endregion 文件验证

            #region 文件上传

            var bytes = FileStreamExtensions.StreamToBytes(input.ImgFile.OpenReadStream());
            var fileFullName = bytes.UploadUFile(fileName);
            if (fileFullName.IsNullOrEmpty())
            {
                res.AddError("UFile PutFile ERROR");
                return res;
            }
            else
            {
                res.Result = fileFullName;
            }

            #endregion 文件上传

            return res;
        }
    }
}