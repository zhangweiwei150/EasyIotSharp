using EasyIotSharp.Domain.Repositories;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Infrastructure.Repositories
{
    public class SqlSugarRepository<T> : IRepository<T> where T : class, new()
    {
        private readonly SqlSugarClient _db;

        public SqlSugarRepository(SqlSugarClient db)
        {
            _db = db;
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _db.Queryable<T>().InSingleAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _db.Queryable<T>().ToListAsync();
        }

        public async Task<List<T>> QueryAsync(Expression<Func<T, bool>> predicate)
        {
            return await _db.Queryable<T>().Where(predicate).ToListAsync();
        }

        public async Task<bool> InsertAsync(T entity)
        {
            return await _db.Insertable(entity).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            return await _db.Updateable(entity).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            return await _db.Deleteable(entity).ExecuteCommandAsync() > 0;
        }
    }
}
