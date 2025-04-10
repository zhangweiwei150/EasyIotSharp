using EasyIotSharp.GateWay.Core.Services;
using EasyIotSharp.GateWay.Core.Socket;
using EasyIotSharp.GateWay.Core.UI;
using EasyIotSharp.GateWay.Core.Util;
using System;
using System.Threading;

namespace EasyIotSharp.GateWay.Core
{
    class Program
    {
        static EasySocketBase easySocketBase = null;

        public static void Main(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.ProcessExit += AppDomain_ProcessExit;
                
                ConsoleUI.ShowBanner();
                
                var serviceManager = new GateWayService();
                serviceManager.InitializeServices();

                var exitEvent = new ManualResetEvent(false);
                Console.CancelKeyPress += (sender, e) =>
                {
                    e.Cancel = true;
                    Console.WriteLine("正在停止服务...");
                    LogHelper.Info("收到停止信号，正在停止服务...");
                    exitEvent.Set();
                };
                
                exitEvent.WaitOne();
                CleanupResources();
            }
            catch (Exception ex)
            {
                LogHelper.Error($"程序启动异常: {ex.ToString()}");
                ConsoleUI.ShowError($"程序启动异常: {ex.Message}");
            }
        }
        
        // 在应用程序退出时关闭RabbitMQ连接
        static void AppDomain_ProcessExit(object sender, EventArgs e)
        {
            LogHelper.Info("应用程序正在退出，清理资源...");
            CleanupResources();
        }
        
        // 清理资源
        static void CleanupResources()
        {
            try
            {
                // 关闭RabbitMQ连接
                try
                {
                    EasyIotSharp.GateWay.Core.LoadingConfig.RabbitMQ.RabbitMQConfig.CloseAllConnections();
                    LogHelper.Info("RabbitMQ连接已关闭");
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"关闭RabbitMQ连接异常: {ex.Message}");
                }
                
                // 其他资源清理...
                
                LogHelper.Info("所有资源已清理完毕");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"资源清理异常: {ex.ToString()}");
            }
        }
    }
}
