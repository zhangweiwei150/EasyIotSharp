using EasyIotSharp.GateWay.Core.Socket;
using System;
using System.Threading;

namespace EasyIotSharp.GateWay.Core
{
    class Program
    {
        static   EasySocketBase easySocketBase = null;

        static void Main(string[] args)
        {
            easySocketBase = new EasyTcpServer();
            easySocketBase.InitTCPServer(new Model.SocketDTO.InitParamsInfo 
            { 
                LocalPort=5020,
                Description="modbusRTU",
                StartState=true,
                Manufacturer="modbusRTU",
                TimeOutMS=3600000
            });
            Console.WriteLine("项目启动.....");

            // 防止程序退出
            var exitEvent = new ManualResetEvent(false);
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                exitEvent.Set();
            };
            
            // 等待 Ctrl+C 信号
            exitEvent.WaitOne();
            
            Console.WriteLine("项目已停止");
        }
    }
}
