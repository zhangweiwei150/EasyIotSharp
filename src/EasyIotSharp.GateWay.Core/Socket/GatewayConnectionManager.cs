using EasyIotSharp.GateWay.Core.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace EasyIotSharp.GateWay.Core.Socket
{
    /// <summary>
    /// 网关连接信息
    /// </summary>
    public class GatewayConnectionInfo
    {
        /// <summary>
        /// 连接ID
        /// </summary>
        public IntPtr ConnId { get; set; }
        
        /// <summary>
        /// 网关ID
        /// </summary>
        public string GatewayId { get; set; }
        
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string IP { get; set; }
        
        /// <summary>
        /// 客户端端口
        /// </summary>
        public ushort Port { get; set; }
        
        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectTime { get; set; }
        
        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActiveTime { get; set; }
        
        /// <summary>
        /// 接收的数据包数量
        /// </summary>
        public int ReceivedPackets { get; set; }
        
        /// <summary>
        /// 接收的总字节数
        /// </summary>
        public long ReceivedBytes { get; set; }
        
        /// <summary>
        /// 最近接收的数据(最多保存20条)
        /// </summary>
        public List<GatewayDataRecord> RecentData { get; set; } = new List<GatewayDataRecord>();
        
        /// <summary>
        /// 是否已注册(收到注册包)
        /// </summary>
        public bool IsRegistered { get; set; }
        
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegisterTime { get; set; }
    }
    
    /// <summary>
    /// 网关数据记录
    /// </summary>
    public class GatewayDataRecord
    {
        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime Time { get; set; }
        
        /// <summary>
        /// 数据内容(十六进制字符串)
        /// </summary>
        public string Data { get; set; }
        
        /// <summary>
        /// 数据长度
        /// </summary>
        public int Length { get; set; }
        
        /// <summary>
        /// 数据类型(如：注册包、心跳包、数据包等)
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 解析结果
        /// </summary>
        public string ParsedResult { get; set; }
    }
    
    /// <summary>
    /// 网关连接管理器
    /// </summary>
    public class GatewayConnectionManager
    {
        private static readonly GatewayConnectionManager _instance = new GatewayConnectionManager();
        
        /// <summary>
        /// 单例实例
        /// </summary>
        public static GatewayConnectionManager Instance => _instance;
        
        /// <summary>
        /// 连接ID到网关连接信息的映射
        /// </summary>
        private readonly ConcurrentDictionary<IntPtr, GatewayConnectionInfo> _connectionMap = 
            new ConcurrentDictionary<IntPtr, GatewayConnectionInfo>();
        
        /// <summary>
        /// 网关ID到连接ID的映射
        /// </summary>
        private readonly ConcurrentDictionary<string, IntPtr> _gatewayMap = 
            new ConcurrentDictionary<string, IntPtr>();
        
        /// <summary>
        /// 添加新连接
        /// </summary>
        public void AddConnection(IntPtr connId, string ip, ushort port)
        {
            var connectionInfo = new GatewayConnectionInfo
            {
                ConnId = connId,
                IP = ip,
                Port = port,
                ConnectTime = DateTime.Now,
                LastActiveTime = DateTime.Now,
                ReceivedPackets = 0,
                ReceivedBytes = 0,
                IsRegistered = false
            };
            
            _connectionMap.TryAdd(connId, connectionInfo);
        }
        
        /// <summary>
        /// 注册网关ID与连接的关联
        /// </summary>
        public void RegisterGateway(IntPtr connId, string gatewayId)
        {
            if (_connectionMap.TryGetValue(connId, out var connectionInfo))
            {
                connectionInfo.GatewayId = gatewayId;
                connectionInfo.IsRegistered = true;
                connectionInfo.RegisterTime = DateTime.Now;
                
                // 更新网关ID到连接ID的映射
                _gatewayMap.AddOrUpdate(gatewayId, connId, (_, __) => connId);
                
                LogHelper.Info($"网关 {gatewayId} 已注册，连接ID: {connId}");
            }
        }
        
        /// <summary>
        /// 更新网关数据
        /// </summary>
        public void UpdateGatewayData(IntPtr connId, byte[] data, string dataType = "数据包")
        {
            if (_connectionMap.TryGetValue(connId, out var connectionInfo))
            {
                connectionInfo.LastActiveTime = DateTime.Now;
                connectionInfo.ReceivedPackets++;
                connectionInfo.ReceivedBytes += data.Length;
                
                // 将数据转换为十六进制字符串
                string hexData = BitConverter.ToString(data).Replace("-", " ");
                
                // 添加数据记录
                var dataRecord = new GatewayDataRecord
                {
                    Time = DateTime.Now,
                    Data = hexData,
                    Length = data.Length,
                    DataType = dataType
                };
                
                // 保持最近20条记录
                connectionInfo.RecentData.Add(dataRecord);
                if (connectionInfo.RecentData.Count > 20)
                {
                    connectionInfo.RecentData.RemoveAt(0);
                }
            }
        }

        // 在 GatewayConnectionManager 类中添加更新解析结果的方法
        /// <summary>
        /// 更新网关数据解析结果
        /// </summary>
        public void UpdateGatewayDataParsedResult(IntPtr connId, byte[] data, string parsedResult)
        {
            if (_connectionMap.TryGetValue(connId, out var connectionInfo))
            {
                string hexData = BitConverter.ToString(data).Replace("-", " ");

                // 查找最近的数据记录
                var dataRecord = connectionInfo.RecentData
                    .FirstOrDefault(r => r.Data == hexData && string.IsNullOrEmpty(r.ParsedResult));

                if (dataRecord != null)
                {
                    dataRecord.ParsedResult = parsedResult;
                }
            }
        }
        /// <summary>
        /// 移除连接
        /// </summary>
        public void RemoveConnection(IntPtr connId)
        {
            if (_connectionMap.TryRemove(connId, out var connectionInfo) && 
                !string.IsNullOrEmpty(connectionInfo.GatewayId))
            {
                _gatewayMap.TryRemove(connectionInfo.GatewayId, out _);
                LogHelper.Info($"网关 {connectionInfo.GatewayId} 连接已断开");
            }
        }
        
        /// <summary>
        /// 通过网关ID获取连接信息
        /// </summary>
        public GatewayConnectionInfo GetConnectionByGatewayId(string gatewayId)
        {
            if (_gatewayMap.TryGetValue(gatewayId, out var connId) && 
                _connectionMap.TryGetValue(connId, out var connectionInfo))
            {
                return connectionInfo;
            }
            
            return null;
        }
        
        /// <summary>
        /// 通过连接ID获取连接信息
        /// </summary>
        public GatewayConnectionInfo GetConnectionByConnId(IntPtr connId)
        {
            _connectionMap.TryGetValue(connId, out var connectionInfo);
            return connectionInfo;
        }
        
        /// <summary>
        /// 获取所有连接信息
        /// </summary>
        public List<GatewayConnectionInfo> GetAllConnections()
        {
            return _connectionMap.Values.ToList();
        }
        
        /// <summary>
        /// 获取所有已注册的网关连接
        /// </summary>
        public List<GatewayConnectionInfo> GetAllRegisteredGateways()
        {
            return _connectionMap.Values.Where(c => c.IsRegistered).ToList();
        }
    }
}