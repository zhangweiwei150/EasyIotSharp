using Confluent.Kafka;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UPrime.Logging;
using EasyIotSharp.Core.Configuration;

namespace EasyIotSharp.Core.Queue.Impl
{
    public class KafkaService : IKafkaService
    {
        private readonly AppOptions _appOptions;

        public KafkaService(AppOptions appOptions)
        {
            _appOptions = appOptions;
        }

        public async Task PublishAsync<TMessage>(string topicName, TMessage message) where TMessage : class
        {
            var config = new ProducerConfig { BootstrapServers = _appOptions.QueueOptions.KafkaBrokerList };
            using var producer = new ProducerBuilder<string, string>(config).Build();
            var res = await producer.ProduceAsync(topicName, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = message.SerializeToJson()
            });
        }

        public void Subscribe<TMessage>(string topicName, Action<TMessage> messageFunc, CancellationToken cancellationToken, int delay = 0) where TMessage : class
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _appOptions.QueueOptions.KafkaBrokerList,
                GroupId = _appOptions.QueueOptions.KafkaGroupId,
                EnableAutoCommit = false,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true
            };

            // https://github.com/confluentinc/confluent-kafka-dotnet/blob/master/examples/Consumer/Program.cs
            using var consumer = new ConsumerBuilder<Ignore, string>(config)
                // Note: All handlers are called on the main .Consume thread.
                .SetErrorHandler((_, e) => Log.Information($"Error: {e.Reason}"))
                .SetStatisticsHandler((_, json) => Log.Information($" - {DateTime.Now:yyyy-MM-dd HH:mm:ss} > 消息监听中..")
                //Log.Information($"Statistics: {json}"
                )
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    string partitionsStr = string.Join(", ", partitions);
                    Log.Information($" - 分配的 kafka 分区: {partitionsStr}");
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    string partitionsStr = string.Join(", ", partitions);
                    Log.Information($" - 回收了 kafka 的分区: {partitionsStr}");
                })
                .Build();
            consumer.Subscribe(topicName);
            try
            {
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        if (consumeResult.IsPartitionEOF)
                        {
                            Log.Information($" - {DateTime.Now:yyyy-MM-dd HH:mm:ss} 已经到底了：{consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");
                            continue;
                        }

                        // consumeResult.TopicPartitionOffset 偏移位置
                        TMessage messageResult;
                        try
                        {
                            messageResult = JsonConvert.DeserializeObject<TMessage>(consumeResult.Message.Value);
                        }
                        catch (Exception ex)
                        {
                            // 序列化失败，记录日志并提交，以免下次重复消费
                            var errorMessage = $" - {DateTime.Now:yyyy-MM-dd HH:mm:ss}【Exception 消息反序列化失败，Value：{consumeResult.Message.Value}】 ：{ex.StackTrace}";
                            Log.Error(errorMessage);
                            LogHelper.LogException(ex);
                            messageResult = null;
                        }
                        if (messageResult == null) continue;
                        messageFunc(messageResult);
                        try
                        {
                            consumer.Commit(consumeResult);
                        }
                        catch (KafkaException e)
                        {
                            LogHelper.LogException(e);
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Log.Error($"Consume error: {e.Error.Reason}");
                        LogHelper.LogException(e);
                    }
                    finally
                    {
                        if (delay > 0)
                            Thread.Sleep(delay);
                    }
                }
            }
            catch (OperationCanceledException e)
            {
                consumer.Close();
                LogHelper.LogException(e);
                Log.Error("Closing consumer.");
            }
        }

        public void Subscribe<TMessage>(string topicName, Action<IList<TMessage>> messageFunc, CancellationToken cancellationToken, int threshold) where TMessage : class
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _appOptions.QueueOptions.KafkaBrokerList,
                GroupId = _appOptions.QueueOptions.KafkaGroupId,
                EnableAutoCommit = false,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true
            };

            // https://github.com/confluentinc/confluent-kafka-dotnet/blob/master/examples/Consumer/Program.cs
            using var consumer = new ConsumerBuilder<Ignore, string>(config)
                // Note: All handlers are called on the main .Consume thread.
                .SetErrorHandler((_, e) => Log.Information($"Error: {e.Reason}"))
                .SetStatisticsHandler((_, json) => Log.Information($" - {DateTime.Now:yyyy-MM-dd HH:mm:ss} > 消息监听中..")
                //Log.Information($"Statistics: {json}"
                )
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    string partitionsStr = string.Join(", ", partitions);
                    Log.Information($" - 分配的 kafka 分区: {partitionsStr}");
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    string partitionsStr = string.Join(", ", partitions);
                    Log.Information($" - 回收了 kafka 的分区: {partitionsStr}");
                })
                .Build();
            consumer.Subscribe(topicName);
            try
            {
                var messages = new List<TMessage>();
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        if (consumeResult.IsPartitionEOF)
                        {
                            Log.Information($" - {DateTime.Now:yyyy-MM-dd HH:mm:ss} 已经到底了：{consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");
                            continue;
                        }

                        // consumeResult.TopicPartitionOffset 偏移位置
                        TMessage messageResult;
                        try
                        {
                            messageResult = JsonConvert.DeserializeObject<TMessage>(consumeResult.Message.Value);
                        }
                        catch (Exception ex)
                        {
                            // 序列化失败，记录日志并提交，以免下次重复消费
                            var errorMessage = $" - {DateTime.Now:yyyy-MM-dd HH:mm:ss}【Exception 消息反序列化失败，Value：{consumeResult.Message.Value}】 ：{ex.StackTrace}";
                            Log.Error(errorMessage);
                            LogHelper.LogException(ex);
                            messageResult = null;
                        }
                        if (messageResult == null) continue;
                        messages.Add(messageResult);
                        if (messages.Count >= threshold)
                        {
                            messageFunc(messages);
                            messages.Clear();
                            try
                            {
                                consumer.Commit(consumeResult);
                            }
                            catch (KafkaException e)
                            {
                                LogHelper.LogException(e);
                            }
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Log.Error($"Consume error: {e.Error.Reason}");
                        LogHelper.LogException(e);
                    }
                }
            }
            catch (OperationCanceledException e)
            {
                consumer.Close();
                LogHelper.LogException(e);
                Log.Error("Closing consumer.");
            }
        }
    }
}