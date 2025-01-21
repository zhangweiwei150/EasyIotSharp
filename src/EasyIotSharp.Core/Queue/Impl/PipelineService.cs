using UPrime.Dependency;
using EasyIotSharp.Core.Configuration;

namespace EasyIotSharp.Core.Queue.Impl
{
    public class PipelineService : IPipelineService, ITransientDependency
    {
        private readonly QueueOptions _queueOptions;
        private readonly IKafkaService _kafkaService;

        public PipelineService(QueueOptions queueOptions, IKafkaService kafkaService)
        {
            _queueOptions = queueOptions;
            _kafkaService = kafkaService;
        }
    }
}