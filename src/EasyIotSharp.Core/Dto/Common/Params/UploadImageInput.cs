using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Core.Dto.Common.Params
{
    public class UploadImageInput
    {
        /// <summary>
        /// 图片文件上传
        /// </summary>
        public IFormFile ImgFile { get; set; }
    }
}