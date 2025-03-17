using HPSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Models
{
    /// <summary>
    /// 任务队列
    /// </summary>
    public class TaskInfo
    {
        /// <summary>
        /// 客户信息
        /// </summary>
        public ClientInfo Client { get; set; }
        public IServer Server { get; set; }
        /// <summary>
        /// 封包
        /// </summary>
        public Packet Packet { get; set; }
    }
}
