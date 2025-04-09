using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.GateWay.Core.Model.SocketDTO
{
    /// <summary>
    /// 客户信息
    /// </summary>
    public class ClientInfo
    {

        public IntPtr ConnId { get; set; }

        public List<byte> lsBytesData { get; set; }

        public string ConfigJson { get; set; }
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
