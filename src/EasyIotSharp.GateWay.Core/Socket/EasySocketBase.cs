using EasyIotSharp.GateWay.Core.Model.SocketDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.GateWay.Core.Socket
{
    public abstract class EasySocketBase
    {
        public abstract void InitTCPServer(InitParamsInfo initParamsInfo);
    }
}
