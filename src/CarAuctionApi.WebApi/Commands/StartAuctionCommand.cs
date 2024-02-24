using CarAuctionApi.Patterns;

namespace CarAuctionApi.WebApi.Commands
{
    public record StartAuctionCommand : ICommand<int>
    {
        public int VehicleId { get; init; }
    }
}
