
namespace Api.Shared.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
        Task<T> AddAsync(T modelo);
        Task<bool> UpdateAsync(T modelo);
        Task<bool> UpdateRange(IQueryable<T> modelo);
        Task<bool> Delete(T modelo);
    }
}
