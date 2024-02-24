using CarAuctionApi.Patterns;

namespace CarAuctionApi.WebApi.Commands
{
    public record PlaceBidCommand : ICommand<int>
    {
        public int VehicleId { get; set; }

        public int CustomerId { get; set; }

        public decimal Value { get; set; }
    }
}
