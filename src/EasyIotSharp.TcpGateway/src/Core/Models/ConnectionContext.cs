using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Models
{
    public class ConnectionContext
    {
        public required string ConnectionId { get; init; }
        public IPEndPoint RemoteEndPoint { get; init; }
        public DateTime LastActivity { get; set; } = DateTime.UtcNow;
        public Dictionary<string, object> Properties { get; } = new();
    }
}
