using EasyIotSharp.GateWay.Core.Model.SocketDTO;
using EasyIotSharp.GateWay.Core.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using EasyIotSharp.GateWay.Core.Socket.Factory;
using EasyIotSharp.GateWay.Core.Domain;
using System.Linq;
using System.Text.Json;
using EasyIotSharp.GateWay.Core.Util.ModbusUtil;

namespace EasyIotSharp.GateWay.Core.Socket.Service
{
    public class ModbusDTU : EasyTCPSuper
    {
        public easyiotsharpContext _easyiotsharpContext;
        public ModbusDTU(easyiotsharpContext easyiotsharpContext)
        {
            _easyiotsharpContext = easyiotsharpContext;
        }
        public override void DecodeData(TaskInfo taskData)
        {
            try
            {

                //1、server查询是否ConfigJson=null
                string modbusConfig = taskData.Client.ConfigJson;
                if (taskData.Packet == null || taskData.Packet.BData == null)
                {
                    LogHelper.Error("taskData.Packet == null || taskData.Packet.BData == null ");
                    return;
                }
                byte[] bReceived = taskData.Packet.BData;//队列的封包数据
                if (!string.IsNullOrEmpty(modbusConfig))
                {
                    //2、如果有则正常解析数据
                    byte[] bData = taskData.Packet.BData;
                    StringBuilder sResult = new StringBuilder();
                    //bool isPushMQ = (modbusDevieConfig.IsPushMq == "1");
                    if (bData == null || bData.Length < 7)
                    {
                        // LogHelper.Error(modbusDevieConfig.DeviceName + " 数据异常：" + ParseUtil.ToHexString(bData, bData.Length));
                        return;
                    }
                    //LogHelper.Info("Pid:" + modbusDevieConfig.Pid + " DeviceName: " + modbusDevieConfig.DeviceName + " 数据=> " + ParseUtil.ToHexString(bData, bData.Length));

                    int length = bData.Length;
                    //byte[] bCRC = DataCheck.ModbusCRC16(bData, 0, bData.Length - 2);
                    //if (bData[length - 2] == bCRC[0] && bData[length - 1] == bCRC[1] || modbusDevieConfig.RegistCode == "dawangqiao102")
                    //{
                    //    //更新HPSocket附加数据配置
                    //    ComModbusDevice deviceLatest = CacheManager.Get<List<ComModbusDevice>>("lsmodbusDevice").Find(s => s.RegistCode == modbusDevieConfig.RegistCode);
                    //    if (deviceLatest != null)
                    //    {
                    //        taskData.Client.ModbusDevieConfig = deviceLatest;
                    //        manufactureTagbase = ManufactureFactory.CreateManufacturer(deviceLatest.Manufacturer);
                    //        if (manufactureTagbase == null)
                    //        {
                    //            LogHelper.Error("SensorTagFactory.CreateManufacturer 工厂创建对象失败！manufacturer = " + manufactureTagbase);
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            manufactureTagbase.PushMQ(deviceLatest, bData);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        LogHelper.Error("CacheManager.Get<List<ComModbusDevie>>(lsmodbusDevice).Find(s => s.RegistCode == modbusDevieConfig.RegistCode)  为空 RegistCode=" + modbusDevieConfig.RegistCode);
                    //    }
                    //}
                }
                else
                {
                    //3.1ModbusDevieConfig未查询到,则进行解码、
                    string strData = System.Text.Encoding.ASCII.GetString(bReceived, 0, bReceived.Length);//转换为Ascll码
                    LogHelper.Info("  收到注册包:  " + strData);
                    var gatewayprotocol = _easyiotsharpContext.Gatewayprotocol.Where(x=>x.GatewayId.Equals(strData)).FirstOrDefault();
                    if (gatewayprotocol == null)
                    {
                        LogHelper.Info("未找到注册包: " + strData);
                        return;
                    }
                    //3.2 查询ComModbusDevie配置、
                    taskData.Client.ConfigJson = gatewayprotocol.ConfigJson;
                    //3.3更新server的Extra
                    taskData.Server.SetExtra(taskData.Client.ConnId, taskData.Client);
                    //3.4 启动Task定时发送采集命令
                    Task tSendCmd = new Task(() => SendMsgToClient("hex", taskData), TaskCreationOptions.LongRunning);
                    tSendCmd.Start();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ModbusDTU 异常:" + ex.ToString());
                return;
            }
        }


        private static void SendMsgToClient(string dataFormart, TaskInfo taskData)
        {
            if (string.IsNullOrEmpty(taskData.Client.ConfigJson))
            {
                LogHelper.Error("ConfigJson为空，无法发送命令");
                return;
            }

            try
            {
                int defaultSleepTime = 20000; // 默认间隔时间
                Dictionary<string, int> intervalMap = new Dictionary<string, int>(); // 存储每个命令的间隔时间
                List<byte[]> commandList = new List<byte[]>();

                using (JsonDocument document = JsonDocument.Parse(taskData.Client.ConfigJson))
                {
                    var root = document.RootElement;
                    if (root.ValueKind != JsonValueKind.Array)
                    {
                        LogHelper.Error("ConfigJson格式错误，应为数组格式");
                        return;
                    }

                    // 解析JSON并生成命令
                    foreach (JsonElement element in root.EnumerateArray())
                    {
                        try
                        {
                            if (!element.TryGetProperty("formData", out JsonElement formData))
                            {
                                LogHelper.Warn("配置项缺少formData字段，跳过");
                                continue;
                            }

                            // 获取必要参数，添加错误处理
                            if (!TryParseJsonValue(formData, "functionCode", out byte functionCode) ||
                                !TryParseJsonValue(formData, "address", out byte slaveAddress) ||
                                !TryParseJsonValue(formData, "StartingAddress", out ushort startAddress) ||
                                !TryParseJsonValue(formData, "Quantity", out ushort quantity))
                            {
                                LogHelper.Warn("配置项缺少必要字段或格式错误，跳过");
                                continue;
                            }

                            // 组装Modbus命令
                            byte[] cmd = new byte[8];
                            cmd[0] = slaveAddress;                  // 从站地址
                            cmd[1] = functionCode;                  // 功能码
                            cmd[2] = (byte)(startAddress >> 8);     // 起始地址高字节
                            cmd[3] = (byte)(startAddress & 0xFF);   // 起始地址低字节
                            cmd[4] = (byte)(quantity >> 8);         // 数量高字节
                            cmd[5] = (byte)(quantity & 0xFF);       // 数量低字节

                            // 计算CRC16
                            byte[] bCRC = ModbusCRC.ModbusCRC16(cmd, 0, 6);
                            cmd[6] = bCRC[0];                       // CRC低字节
                            cmd[7] = bCRC[1];                       // CRC高字节
                            
                            commandList.Add(cmd);
                            
                            // 获取间隔时间
                            if (formData.TryGetProperty("Interval", out JsonElement intervalElement) && 
                                int.TryParse(intervalElement.GetString(), out int interval))
                            {
                                int sleepTime = interval * 1000; // 转换为毫秒
                                intervalMap[BitConverter.ToString(cmd)] = sleepTime;
                                
                                // 使用最小的间隔时间作为全局间隔
                                if (sleepTime < defaultSleepTime)
                                {
                                    defaultSleepTime = sleepTime;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error($"解析配置项时发生错误: {ex.Message}");
                        }
                    }
                }

                if (commandList.Count == 0)
                {
                    LogHelper.Error("没有有效的命令可发送");
                    return;
                }

                // 发送命令的主循环
                Task.Run(() =>
                {
                    try
                    {
                        while (true)
                        {
                            foreach (var cmd in commandList)
                            {
                                try
                                {
                                    // 发送命令
                                    bool isok = taskData.Server.Send(taskData.Client.ConnId, cmd, cmd.Length);
                                    string cmdHex = BitConverter.ToString(cmd).Replace("-", " ");
                                    
                                    if (isok)
                                    {
                                        int cmdSleepTime = intervalMap.TryGetValue(BitConverter.ToString(cmd), out int time) ? time : defaultSleepTime;
                                        LogHelper.Info($"发送命令成功: {cmdHex}, 间隔: {cmdSleepTime}ms");
                                    }
                                    else
                                    {
                                        LogHelper.Error($"发送命令失败: {cmdHex}");
                                        return;
                                    }
                                    
                                    // 命令间隔
                                    Thread.Sleep(2000);
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error($"发送命令时发生错误: {ex.Message}");
                                }
                            }
                            
                            // 全局间隔
                            Thread.Sleep(defaultSleepTime);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error($"命令发送循环中断: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error($"SendMsgToClient() 发生异常: {ex.Message}");
            }
        }

        // 辅助方法：尝试从JSON中解析值
        private static bool TryParseJsonValue<T>(JsonElement element, string propertyName, out T value)
        {
            value = default;
            
            if (!element.TryGetProperty(propertyName, out JsonElement property))
                return false;
                
            try
            {
                if (typeof(T) == typeof(byte))
                {
                    if (byte.TryParse(property.GetString(), out byte byteValue))
                    {
                        value = (T)(object)byteValue;
                        return true;
                    }
                }
                else if (typeof(T) == typeof(ushort))
                {
                    if (ushort.TryParse(property.GetString(), out ushort ushortValue))
                    {
                        value = (T)(object)ushortValue;
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            
            return false;
        }
    }
}
