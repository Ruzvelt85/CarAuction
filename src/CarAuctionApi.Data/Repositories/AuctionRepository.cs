using CarAuctionApi.Domain.Model;

namespace CarAuctionApi.Data.Repositories
{
    public class AuctionRepository : BaseRepository<Auction>, IAuctionRepository
    {
        public AuctionRepository(InMemoryEfCoreContext context) : base(context)
        {
        }
    }
}
