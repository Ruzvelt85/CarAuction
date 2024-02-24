using CarAuctionApi.Patterns;

namespace CarAuctionApi.Data
{
    public sealed class UnitOfWork<TDbContext> : IUnitOfWork
        where TDbContext : InMemoryEfCoreContext
    {
        private readonly TDbContext _context;

        public UnitOfWork(TDbContext efCoreDbContext)
        {
            _context = efCoreDbContext ?? throw new ArgumentNullException(nameof(efCoreDbContext));
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
