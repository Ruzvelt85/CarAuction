namespace CarAuctionApi.Dto.Responses;

public record VehicleListResponseDto
{
    public IReadOnlyCollection<VehicleResponseDto> Items { get; init; } = new List<VehicleResponseDto>();
}
