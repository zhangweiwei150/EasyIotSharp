using EasyIotSharp.GateWay.Core.Domain;
using EasyIotSharp.GateWay.Core.Model.AnalysisDTO;
using EasyIotSharp.GateWay.Core.Services;
using EasyIotSharp.GateWay.Core.Socket;
using System;
using System.Linq;
using System.Text.Json;

namespace EasyIotSharp.GateWay.Core.Util
{
    public static class DataParserHelper
    {
        public static void SendEncryptedData(
            LowFrequencyData sensorData,
            GatewayConnectionInfo connectionInfo,
            easyiotsharpContext dbContext,
            RabbitMQService rabbitMQService)
        {
            if (string.IsNullOrEmpty(connectionInfo?.GatewayId)) return;

            var gateway = dbContext.Gateway.FirstOrDefault(g => g.Id == connectionInfo.GatewayId);
            if (gateway != null && !string.IsNullOrEmpty(gateway.ProjectId))
            {
                try
                {
                    string encryptedData = sensorData.ToEncryptedString();
                    rabbitMQService.SendMessage(gateway.ProjectId, encryptedData);
                    LogHelper.Info($"发送加密数据到RabbitMQ成功, 网关ID: {connectionInfo.GatewayId}");
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"发送到RabbitMQ失败: {ex.ToString()}");
                }
            }
        }


    }
}