using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Cloud.TcpGateway.src.Infrastructure.Networking
{
    public class TcpServerOptions
    {
        public int Port { get; set; } = 5020;
        public int BufferSize { get; set; } = 8192;
        public int MaxConnections { get; set; } = 1000;
        public int SilenceTimeout { get; set; } = 30000;
    }
}
