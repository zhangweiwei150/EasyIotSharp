using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.GateWay.Core.Model.SocketDTO
{
    public class InitParamsInfo
    {
        public int LocalPort { get; set; }//本地端口
        public string Description { get; set; }//描述信息
        public bool StartState { get; set; }//开始默认状态
        public uint TimeOutMS { get; set; }//超时时间 ms
        public string Manufacturer { get; set; }//厂商
    }
}
