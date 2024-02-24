namespace CarAuctionApi.Dto.Requests;

public record PlaceBidRequestDto
{
    public int VehicleId { get; set; }

    public int CustomerId { get; set; }

    public decimal Value { get; set; }
}
