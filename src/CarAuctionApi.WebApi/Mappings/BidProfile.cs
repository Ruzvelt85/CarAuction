using AutoMapper;
using CarAuctionApi.Domain.Model;
using CarAuctionApi.Dto.Requests;
using CarAuctionApi.WebApi.Commands;

namespace CarAuctionApi.WebApi.Mappings
{
    public class BidProfile : Profile
    {
        public BidProfile()
        {
            CreateMap<PlaceBidRequestDto, PlaceBidCommand>();
            CreateMap<PlaceBidCommand, Bid>(MemberList.Source);
        }
    }
}
