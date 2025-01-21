using System;
using System.IO;
using UPrime;

namespace EasyIotSharp.Core.Services.IO.Impl
{
    public class TempFolderService : ITempFolderService
    {
        /// <summary>
        /// 临时文件夹应用绝对路径
        /// </summary>
        public string Path
        {
            get
            {
                var path = Directory.GetCurrentDirectory() + "\\App_Data\\Temp";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }

        /// <summary>
        /// 临时文件夹生成Pdf的应用绝对路径
        /// </summary>
        public string GeneratePdfPath
        {
            get
            {
                var path = Path + "\\GeneratePdf";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }

        /// <summary>
        /// 通过临时文件夹路径获取绝对路径
        /// </summary>
        /// <param name="ext">后缀名（如：.txt等）</param>
        /// <returns></returns>
        public string GetFileWithPath(string ext)
        {
            string path = "{0}\\{1}_{2}{3}".FormatWith(Path, DateTime.Now.ToString("yyMMdd"), Guid.NewGuid().ToString().ReplaceByEmpty("-"), ext);

            return path;
        }

        /// <summary>
        /// 从临时文件夹删除文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionOutput DeleteFile(string fileName)
        {
            ActionOutput res = new ActionOutput();
            try
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
                else
                {
                    var path = Path + "\\" + fileName;
                    if (File.Exists(path))
                        File.Delete(path);
                    else
                    {
                        path = GeneratePdfPath + "\\" + fileName;
                        if (File.Exists(path))
                            File.Delete(path);
                    }
                }
            }
            catch (Exception ex)
            {
                res.AddError(ex.Message);
            }

            return res;
        }
    }
}