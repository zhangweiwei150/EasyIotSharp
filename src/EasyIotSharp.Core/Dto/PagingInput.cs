namespace EasyIotSharp.Core.Dto
{
    /// <summary>
    /// 分页基类 其他需分页类的Input须集成该类
    /// </summary>
    public class PagingInput
    {
        /// <summary>
        /// 当前开始页
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 是否分页
        /// </summary>
        public bool IsPage { get; set; } = true;
    }
}