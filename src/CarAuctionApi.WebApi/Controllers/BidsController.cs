using AutoMapper;
using CarAuctionApi.Dto.Requests;
using CarAuctionApi.Patterns;
using CarAuctionApi.ServiceInfrastructure.Filters;
using CarAuctionApi.WebApi.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CarAuctionApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandHandler<PlaceBidCommand, int> _placeBidCommandHandler;

        public BidsController(IMapper mapper, ICommandHandler<PlaceBidCommand, int> placeBidCommandHandler)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _placeBidCommandHandler = placeBidCommandHandler ?? throw new ArgumentNullException(nameof(placeBidCommandHandler));
        }

        [HttpPost]
        [ServiceFilter(typeof(SaveChangesActionFilterAttribute))]
        public async Task<int?> PlaceBidAsync([FromBody] PlaceBidRequestDto request)
        {
            var command = _mapper.Map<PlaceBidCommand>(request);
            var result = await _placeBidCommandHandler.HandleAsync(command);
            return result;
        }
    }
}
