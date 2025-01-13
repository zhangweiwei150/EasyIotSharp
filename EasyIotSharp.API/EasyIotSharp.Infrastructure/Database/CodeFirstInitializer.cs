using EasyIotSharp.Infrastructure.Database.Entities;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyIotSharp.Infrastructure.Database
{
    /// <summary>
    /// SqlSugar CodeFirst 表初始化器
    /// </summary>
    public class CodeFirstInitializer
    {
        private readonly SqlSugarClient _dbClient;
        private readonly IConfiguration _configuration;

        public CodeFirstInitializer(SqlSugarDbContext dbContext, IConfiguration configuration)
        {
            _dbClient = dbContext.Database;
            _configuration = configuration;
        }

        public void InitializeTables()
        {
            // 从配置中读取是否启用 CodeFirst
            bool enableCodeFirst = _configuration.GetValue<bool>("DatabaseSettings:EnableCodeFirst");

            if (!enableCodeFirst)
            {
                Console.WriteLine("CodeFirst 表初始化已被禁用。");
                return;
            }

            try
            {
                _dbClient.CodeFirst.InitTables(
                    typeof(User),
                    typeof(Tenant),
                    typeof(Role),
                    typeof(UserRole),
                    typeof(Menu),
                    typeof(RoleMenu)
                );

                Console.WriteLine("数据库表已成功初始化！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"数据库表初始化失败：{ex.Message}");
            }
        }
    }
}
