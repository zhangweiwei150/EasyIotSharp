using Microsoft.Extensions.Configuration;

namespace EasyIotSharp.Core.Configuration
{
    /// <summary>
    /// 消息中间件配置项
    /// - Kafka
    /// </summary>
    public class QueueOptions
    {
        /// <summary>
        /// Kafka：是否开启
        /// </summary>
        public bool KafkaEnabled { get; internal set; }

        /// <summary>
        /// Kafka：broker 服务列表
        ///  ex: x.x.x.x:9092
        /// </summary>
        public string KafkaBrokerList { get; internal set; }

        /// <summary>
        /// Kafka：订阅组标识
        /// </summary>
        public string KafkaGroupId { get; internal set; }

        public static QueueOptions ReadFromConfiguration(IConfiguration config)
        {
            QueueOptions options = new QueueOptions();
            var cs = config.GetSection("Queue");
            options.KafkaEnabled = cs.GetValue<bool>(nameof(KafkaEnabled));
            options.KafkaBrokerList = cs.GetValue<string>(nameof(KafkaBrokerList));
            options.KafkaGroupId = cs.GetValue<string>(nameof(KafkaGroupId));
            return options;
        }
    }
}