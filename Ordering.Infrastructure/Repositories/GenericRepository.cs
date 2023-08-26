using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Ordering.Infrastructure.Repositories
{
    public class GenericRepository<T> where T : EntityBase
    {
        private readonly OrderContext orderContext;

        public GenericRepository(OrderContext orderContext)
        {
            this.orderContext = orderContext;
        }

        public async Task<List<T>> GetallAsync()
            => await orderContext.Set<T>().ToListAsync();

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate)
            => await orderContext.Set<T>().Where(predicate).ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await orderContext.Set<T>().FindAsync(id);

        public async Task<T> AddAsync(T item)
        {
            await orderContext.Set<T>().AddAsync(item);
            await orderContext.SaveChangesAsync();

            return item;
        }

        public async Task UpdateAsync(T item)
        {
            orderContext.Entry(item).State = EntityState.Modified;

            await orderContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            orderContext.Set<T>().Remove(entity);
            await orderContext.SaveChangesAsync();
        }
    }
}
