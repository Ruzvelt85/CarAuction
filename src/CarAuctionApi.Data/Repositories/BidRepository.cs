using CarAuctionApi.Domain.Model;

namespace CarAuctionApi.Data.Repositories
{
    public class BidRepository : BaseRepository<Bid>, IBidRepository
    {
        public BidRepository(InMemoryEfCoreContext context) : base(context)
        {
        }

        public decimal? GetMaxBid(int vehicleId)
        {
            var bids = DbSet.Where(bid => bid.VehicleId == vehicleId);
            if (!bids.Any())
            {
                return null;
            }

            return  bids.Max(bid => bid.Value);
        }

        public async Task DeleteRangeAsync(IEnumerable<Bid> entities)
        {
            Context.Set<Bid>().RemoveRange(entities);
            await Task.CompletedTask;
        }
    }
}
