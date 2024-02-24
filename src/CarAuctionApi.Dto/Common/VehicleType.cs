using System.Text.Json.Serialization;

namespace CarAuctionApi.Dto.Common;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VehicleType
{
    Sedan = 1,

    Hatchback = 2,

    SUV = 3,

    Truck = 4,
}
