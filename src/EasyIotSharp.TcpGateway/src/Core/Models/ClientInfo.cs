using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Models
{
    /// <summary>
    /// 客户信息
    /// </summary>
    public class ClientInfo
    {

        public IntPtr ConnId { get; set; }

        public List<byte> lsBytesData { get; set; }

    }


    /// <summary>
    /// 客户信息
    /// </summary>
    public class ClientInfo<TDataType> : ClientInfo
    {
        /// <summary>
        /// 封包数据
        /// </summary>
        public TDataType PacketData { get; set; }
    }
}
