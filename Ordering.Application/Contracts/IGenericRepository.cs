using Ordering.Domain.Common;
using System.Linq.Expressions;

namespace Ordering.Application.Contracts
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        Task<T> AddAsync(T item);
        Task DeleteAsync(T entity);
        Task<List<T>> GetallAsync();
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(int id);
        Task UpdateAsync(T item);
    }
}