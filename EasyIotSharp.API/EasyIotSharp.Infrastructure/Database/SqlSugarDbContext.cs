using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Infrastructure.Database
{
    public class SqlSugarDbContext
    {
        private string connectionString = null;

        public SqlSugarDbContext() : this(Config.AppSettings["DBConnection"])
        {

        }
        public SqlSugarDbContext(string connectionString)
        {
            this.connectionString = connectionString;
            this.Database = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = this.connectionString,
                DbType = DbType.MySql,// 数据库类型，根据实际情况修改
                IsAutoCloseConnection = true,// 自动关闭连接
                InitKeyType = InitKeyType.Attribute// 使用特性映射
            });
            this.Database.Ado.IsEnableLogEvent = true;
            this.Database.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
            {

            };
            this.Database.Aop.OnError = (exp) =>//执行SQL 错误事件
            {
            };
            this.Database.Aop.OnDiffLogEvent = (it) =>
            {
            };
            this.Database.Aop.OnLogExecuted = (sql, pars) => //SQL执行完事件
            {

            };
        }

        public SqlSugarClient Database { get; private set; }
    }
}
