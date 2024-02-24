using CarAuctionApi.Domain.Model;

namespace CarAuctionApi.Data.Repositories
{
    public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(InMemoryEfCoreContext context) : base(context)
        {
        }
    }
}
