﻿using EasyIotSharp.GateWay.Core.Util.ModbusUtil;
using EasyIotSharp.GateWay.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;
using EasyIotSharp.GateWay.Core.Domain;
using System.Linq;
using EasyIotSharp.GateWay.Core.Model.RaddbitDTO;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIotSharp.GateWay.Core.LoadingConfig.RabbitMQ
{
    /// <summary>
    /// RabbitMQ配置管理类
    /// 负责初始化和管理RabbitMQ连接
    /// </summary>
    public class RabbitMQConfig
    {
        /// <summary>
        /// 路由键映射表
        /// </summary>
        public static Dictionary<string, string> m_RoutingKey = new Dictionary<string, string>();
        
        /// <summary>
        /// 项目ID到MQ客户端的映射
        /// </summary>
        public static Dictionary<string, RabbitMQClient> dicPid2MQClient = new Dictionary<string, RabbitMQClient>();
        
        /// <summary>
        /// MQ客户端列表
        /// </summary>
        private static List<RabbitMQClient> lsMQs = new List<RabbitMQClient>();
        
        /// <summary>
        /// 是否已初始化
        /// </summary>
        private static bool _isInitialized = false;
        
        /// <summary>
        /// 初始化锁对象
        /// </summary>
        private static readonly object _initLock = new object();
        
        /// <summary>
        /// 初始化RabbitMQ配置
        /// </summary>
        // 修改查询和初始化逻辑
        public static void InitMQ(IServiceProvider serviceProvider = null)
        {
            // 防止重复初始化
            if (_isInitialized)
            {
                LogHelper.Info("RabbitMQ已经初始化，跳过重复初始化");
                return;
            }
            
            lock (_initLock)
            {
                if (_isInitialized) return;
                
                try
                {
                    // 获取数据库上下文
                    easyiotsharpContext easyiotsharpContext;
                    if (serviceProvider != null)
                    {
                        easyiotsharpContext = serviceProvider.GetRequiredService<easyiotsharpContext>();
                    }
                    else
                    {
                        easyiotsharpContext = new easyiotsharpContext();
                    }

                    LogHelper.Info("开始初始化RabbitMQ配置...");
                    
                    // 清空现有配置
                    lsMQs.Clear();
                    dicPid2MQClient.Clear();
                    m_RoutingKey.Clear();

                    // 获取 MQ 配置并初始化 MQ 客户端
                    try
                    {
                        // 查询MQ服务器和项目配置信息
                        var mqlist = (from mqpject in easyiotsharpContext.RabbitProject
                                      join mqserver in easyiotsharpContext.RabbitServerinfo
                                      on mqpject.MqId equals mqserver.Id
                                      select new RabbitMqInfo
                                      {
                                          Host = mqserver.Host,
                                          Port = mqserver.Port,
                                          Username = mqserver.Username,
                                          Password = mqserver.Password,
                                          Virtualhost = mqserver.Virtualhost,
                                          Exchange = mqpject.ProjectId.ToString(),
                                          MqId = mqserver.Id,
                                          ProjectId = mqpject.ProjectId,
                                          // 使用Extendstr作为RoutingKey，如果为空则使用项目ID
                                          RoutingKey = !string.IsNullOrEmpty(mqpject.Extendstr) ? 
                                                  mqpject.Extendstr : mqpject.ProjectId.ToString()
                                      }).ToList();

                        LogHelper.Info($"找到 {mqlist.Count} 个RabbitMQ配置");

                        if (mqlist.Count > 0)
                        {
                            foreach (var item in mqlist)
                            {
                                try
                                {
                                    // 创建并初始化MQ客户端
                                    RabbitMQClient m_MQClient = new RabbitMQClient();
                                    m_MQClient.Host = item.Host;
                                    m_MQClient.Port = item.Port;
                                    m_MQClient.UserName = item.Username;
                                    m_MQClient.Password = item.Password;
                                    m_MQClient.VirtualHost = item.Virtualhost;
                                    m_MQClient.Exchange = item.Exchange;
                                    m_MQClient.mqid = item.MqId;
                                    
                                    // 初始化连接
                                    m_MQClient.Init();
                                    lsMQs.Add(m_MQClient);
                                    
                                    // 添加项目ID到MQ客户端的映射
                                    if (!dicPid2MQClient.ContainsKey(item.ProjectId))
                                    {
                                        dicPid2MQClient.Add(item.ProjectId, m_MQClient);
                                    }
                                    
                                    // 添加路由键映射
                                    string key = item.ProjectId.ToString();
                                    if (!m_RoutingKey.ContainsKey(key))
                                    {
                                        m_RoutingKey.Add(key, item.RoutingKey);
                                    }
                                    
                                    LogHelper.Info($"成功初始化RabbitMQ客户端: {item.Host}:{item.Port}, 项目ID: {item.ProjectId}, 路由键: {item.RoutingKey}");
                                }
                                catch (Exception ex)
                                {
                                    // 单个 MQ 初始化失败，不影响其他
                                    LogHelper.Error($"MQ Client 初始化失败: {ex.ToString()}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error($"获取MQ配置失败: {ex.ToString()}");
                    }
                    
                    _isInitialized = true;
                    LogHelper.Info($"RabbitMQ配置初始化完成，共初始化 {lsMQs.Count} 个客户端");
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"RabbitMQ初始化过程中发生异常: {ex.ToString()}");
                }
            }
        }
        
        /// <summary>
        /// 获取指定项目ID的RabbitMQ客户端
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns>RabbitMQ客户端实例，如果不存在则返回null</returns>
        public static RabbitMQClient GetMQClient(string projectId)
        {
            if (dicPid2MQClient.TryGetValue(projectId, out RabbitMQClient client))
            {
                return client;
            }
            return null;
        }
        
        /// <summary>
        /// 获取指定项目ID的路由键
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns>路由键，如果不存在则返回空字符串</returns>
        public static string GetRoutingKey(string projectId)
        {
            string key = projectId.ToString();
            if (m_RoutingKey.TryGetValue(key, out string routingKey))
            {
                return routingKey;
            }
            return string.Empty;
        }
        
        /// <summary>
        /// 关闭所有RabbitMQ连接
        /// </summary>
        public static void CloseAllConnections()
        {
            foreach (var mqClient in lsMQs)
            {
                try
                {
                    mqClient.Close();
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"关闭RabbitMQ连接失败: {ex.Message}");
                }
            }
            
            lsMQs.Clear();
            dicPid2MQClient.Clear();
            m_RoutingKey.Clear();
            _isInitialized = false;
            
            LogHelper.Info("已关闭所有RabbitMQ连接");
        }
    }
}
