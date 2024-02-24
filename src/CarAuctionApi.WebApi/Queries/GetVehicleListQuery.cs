using CarAuctionApi.Dto.Common;
using CarAuctionApi.Patterns;

namespace CarAuctionApi.WebApi.Queries
{
    public record GetVehicleListQuery : IQuery
    {
        public VehicleType? Type { get; set; }

        public string? Manufacturer { get; set; }

        public string? Model { get; set; }

        public int? Year { get; set; }
    }
}
