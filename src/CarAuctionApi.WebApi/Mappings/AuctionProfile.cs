using AutoMapper;
using CarAuctionApi.Domain.Model;
using CarAuctionApi.Dto.Requests;
using CarAuctionApi.WebApi.Commands;

namespace CarAuctionApi.WebApi.Mappings
{
    public class AuctionProfile : Profile
    {
        public AuctionProfile()
        {
            CreateMap<StartAuctionRequestDto, StartAuctionCommand>();
            CreateMap<StartAuctionCommand, Auction>(MemberList.Source)
                .ForMember(dest => dest.IsActive, opt =>
                    opt.MapFrom(src => true));

            CreateMap<CloseAuctionRequestDto, CloseAuctionCommand>();
        }
    }
}
