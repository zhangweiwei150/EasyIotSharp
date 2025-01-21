using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIotSharp.Core.Queue
{
    /// <summary>
    /// Kafka 消息中间件服务，处理消息的发送与订阅
    /// </summary>
    public interface IKafkaService
    {
        /// <summary>
        /// 发送消息至指定主题
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="topicName"></param>
        /// <param name="message">message value</param>
        /// <returns></returns>
        Task PublishAsync<TMessage>(string topicName, TMessage message) where TMessage : class;

        /// <summary>
        /// 从指定的主题订阅消息
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="messageFunc">消费的回调函数</param>
        /// <param name="cancellationToken">线程取消通知</param>
        /// <param name="delay">延迟</param>
        /// <returns></returns>
        void Subscribe<TMessage>(string topicName, Action<TMessage> messageFunc, CancellationToken cancellationToken, int delay = 0) where TMessage : class;

        /// <summary>
        /// 从指定的主题订阅消息，并在达到一定条数时才触发回调函数
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="messageFunc">消费的回调函数</param>
        /// <param name="cancellationToken">线程取消通知</param>
        /// <param name="threshold">达到触发回调函数的消息数阈值</param>
        /// <returns></returns>
        void Subscribe<TMessage>(string topicName, Action<IList<TMessage>> messageFunc, CancellationToken cancellationToken, int threshold) where TMessage : class;
    }
}