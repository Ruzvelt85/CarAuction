using CarAuctionApi.Domain.Model;
using CarAuctionApi.Patterns;

namespace CarAuctionApi.Data.Repositories
{
    public interface IBidRepository : IRepository<Bid>
    {
        decimal? GetMaxBid(int vehicleId);

        Task DeleteRangeAsync(IEnumerable<Bid> entities);
    }
}
