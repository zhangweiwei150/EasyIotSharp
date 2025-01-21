using SqlSugar;
using SqlSugar.Extensions;
using UPrime;
using EasyIotSharp.Core.Configuration;
using EasyIotSharp.Core.Repositories.Mysql;
using System.Reflection;
using EasyIotSharp.Core.Domain;
using System;
using System.Linq;

namespace EasyIotSharp.Repositories.Mysql
{
    public class SqlSugarDatabaseProvider : ISqlSugarDatabaseProvider
    {
        public SqlSugarDatabaseProvider()
        {
            var storageOptions = UPrimeEngine.Instance.Resolve<StorageOptions>();

            string connectionString = "";
            if (storageOptions.MysqlConnectionMode.ToLower() == "replicaset")
            {
                string servers = "";
                foreach (var item in storageOptions.MysqlServers)
                {
                    servers += $"{item.Host}:{item.Port},";
                }
                servers = servers.Substring(0, servers.Length - 1);
                connectionString = $"Server={servers};Database={storageOptions.MysqlDbName};Uid={storageOptions.MysqlUsername};Pwd={storageOptions.MysqlPassword};";
            }
            else
            {
                connectionString = $"Server={storageOptions.MysqlServers[0].Host};Port={storageOptions.MysqlServers[0].Port};Database={storageOptions.MysqlDbName};Uid={storageOptions.MysqlUsername};Pwd={storageOptions.MysqlPassword};";
            }
            Client = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = SqlSugar.DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            InitializeDatabase(Client);
        }

        public ISqlSugarClient Client { get; }

        private static void InitializeDatabase(ISqlSugarClient db)
        {
            db.DbMaintenance.CreateDatabase();

            // 获取所有继承自 BaseEntity<> 的类型
            var types = Assembly.GetExecutingAssembly().GetTypes()
                                .Where(type => !type.IsAbstract && type.BaseType != null &&
                                               type.BaseType.IsGenericType &&
                                               type.BaseType.GetGenericTypeDefinition() == typeof(BaseEntity<>));

            // 初始化表结构
            foreach (var type in types)
            {
                db.CodeFirst.InitTables(type);
            }
        }
    }
}