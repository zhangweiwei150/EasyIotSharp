using HPSocket;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace EasyIotSharp.GateWay.Core.Model.SocketDTO
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
        public IAgent Agent { get; set; }
        public InitParamsInfo _initParamsInfo { get; set; }
        /// <summary>
        /// 封包
        /// </summary>
        public Packet Packet { get; set; }
    }
}
