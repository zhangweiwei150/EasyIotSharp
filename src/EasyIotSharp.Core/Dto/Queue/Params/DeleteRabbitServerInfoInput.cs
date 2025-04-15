namespace EasyIotSharp.Core.Dto.Queue.Params
{
    /// <summary>
    /// 删除一条RabbitMQ服务器配置信息的入参类
    /// </summary>
    public class DeleteRabbitServerInfoInput
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string Id { get; set; }
    }
}