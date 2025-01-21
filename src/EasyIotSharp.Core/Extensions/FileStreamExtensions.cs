using System.IO;
using UPrime;
using UPrime.SDK.UCloudUFile;
using UPrime.SDK.UCloudUFile.Dto;

namespace EasyIotSharp.Core.Extensions
{
    /// <summary>
    /// 文件流处理扩展
    /// </summary>
    public static class FileStreamExtensions
    {
        /// <summary>
        /// Stream 转换 byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 上传文件至UFile
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="fileName"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public static string UploadUFile(this byte[] buffer, string fileName, string bucket = "")
        {
            var ufileService = UPrimeEngine.Instance.Resolve<IUFileService>();
            var fileExt = Path.GetExtension(fileName);
            int retry = 0;
            PutFileOutput putFileResult = null;
            //重试5次
            while (retry <= 5)
            {
                putFileResult = ufileService.PutFile(buffer, fileExt, fileName, bucket);
                if (putFileResult.Success) break;
                retry++;
            }
            if (putFileResult.IsNotNull() && putFileResult.Success)
                return putFileResult.SourceUrl.ReplaceByEmpty("https:");
            else
                return null;
        }
    }
}