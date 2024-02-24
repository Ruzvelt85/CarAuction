using CarAuctionApi.Dto.Common;

namespace CarAuctionApi.Dto.Requests;

public record GetVehicleListRequestDto
{
    public VehicleType? Type { get; set; }

    public string? Manufacturer { get; set; }

    public string? Model { get; set; }

    public int? Year { get; set; }
}
