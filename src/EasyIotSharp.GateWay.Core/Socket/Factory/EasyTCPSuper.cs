using EasyIotSharp.GateWay.Core.Model.SocketDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.GateWay.Core.Socket.Factory
{
    public abstract class EasyTCPSuper
    {
        public abstract void DecodeData(TaskInfo taskData);
    }
}
