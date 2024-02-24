using System.Linq.Expressions;

namespace CarAuctionApi.Patterns
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IList<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
