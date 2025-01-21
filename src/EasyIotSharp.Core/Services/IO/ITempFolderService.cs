using UPrime;
using UPrime.Dependency;

namespace EasyIotSharp.Core.Services.IO
{
    public interface ITempFolderService : ISingletonDependency
    {
        /// <summary>
        /// 临时文件夹应用绝对路径
        /// </summary>
        string Path { get; }

        /// <summary>
        ///  临时文件夹导出的Excel应用绝对路径
        /// </summary>
        string GeneratePdfPath { get; }

        /// <summary>
        /// 通过临时文件夹路径获取绝对路径
        /// </summary>
        /// <param name="ext">后缀名（如：.txt）</param>
        /// <returns></returns>
        string GetFileWithPath(string ext);

        /// <summary>
        /// 从临时文件夹删除文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        ActionOutput DeleteFile(string fileName);
    }
}