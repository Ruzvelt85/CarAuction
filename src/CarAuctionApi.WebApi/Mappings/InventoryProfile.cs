using AutoMapper;
using CarAuctionApi.Domain.Model;
using CarAuctionApi.Dto.Requests;
using CarAuctionApi.Dto.Responses;
using CarAuctionApi.WebApi.Commands;
using CarAuctionApi.WebApi.Queries;
using VehicleType = CarAuctionApi.Dto.Common.VehicleType;

namespace CarAuctionApi.WebApi.Mappings
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<CreateVehicleCommand, Vehicle>();

            CreateMap<GetVehicleListRequestDto, GetVehicleListQuery>();
            CreateMap<CreateVehicleRequestDto, CreateVehicleCommand>();

            CreateMap<Vehicle, VehicleResponseDto>();
            CreateMap<IEnumerable<Vehicle>, VehicleListResponseDto>()
                .ForMember(dest => dest.Items, opt =>
                    opt.MapFrom(src => new List<Vehicle>(src)));

            CreateMap<VehicleType, Domain.Model.VehicleType>();
            CreateMap<Domain.Model.VehicleType, VehicleType>();
        }
    }
}
