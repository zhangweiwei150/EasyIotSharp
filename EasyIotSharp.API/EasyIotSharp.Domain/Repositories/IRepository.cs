using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EasyIotSharp.Domain.Repositories
{
    public interface IRepository<T> where T : class, new()
    {
        Task<T> GetByIdAsync(object id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> QueryAsync(Expression<Func<T, bool>> predicate);
        Task<bool> InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
