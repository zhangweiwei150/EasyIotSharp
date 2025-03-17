using EasyIotSharp.Cloud.TcpGateway.src.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Cloud.TcpGateway.src.Core.Abstractions
{
    public interface IProtocolProcessor
    {
        Task ProcessAsync(ConnectionContext context, byte[] data);
    }

}
