using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Models
{
    /// <summary>
    /// 封包
    /// </summary>
    public class Packet
    {
        public PacketType Type { get; set; } // 封包类型

        public byte[] BData { get; set; }  // 数据
    }
}
