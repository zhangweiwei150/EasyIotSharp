using SqlSugar;

namespace EasyIotSharp.Core.Repositories.Mysql
{
    public interface ISqlSugarDatabaseProvider
    {
        public ISqlSugarClient Client { get; }
    }
}
