using CarAuctionApi.Patterns;

namespace CarAuctionApi.WebApi.Commands
{
    public record CloseAuctionCommand : ICommand<int>
    {
        public int VehicleId { get; init; }
    }
}
