using CarAuctionApi.Dto.Common;

namespace CarAuctionApi.Dto.Requests;

public record CreateVehicleRequestDto
{
    public int Id { get; set; }
    
    public VehicleType Type { get; set; }

    public string Manufacturer { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public decimal StartingBid { get; set; }

    public int? DoorNumber { get; set; }

    public int? SeatNumber { get; set; }

    public float? LoadCapacity { get; set; }
}
