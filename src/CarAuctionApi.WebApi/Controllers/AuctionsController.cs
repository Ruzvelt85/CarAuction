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
    public class AuctionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandHandler<StartAuctionCommand, int> _startAuctionCommandHandler;
        private readonly ICommandHandler<CloseAuctionCommand, int> _closeAuctionCommandHandler;

        public AuctionsController(IMapper mapper, ICommandHandler<StartAuctionCommand, int> startAuctionCommandHandler, ICommandHandler<CloseAuctionCommand, int> closeAuctionCommandHandler)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _startAuctionCommandHandler = startAuctionCommandHandler ?? throw new ArgumentNullException(nameof(startAuctionCommandHandler));
            _closeAuctionCommandHandler = closeAuctionCommandHandler ?? throw new ArgumentNullException(nameof(closeAuctionCommandHandler));
        }

        [HttpPost]
        [ServiceFilter(typeof(SaveChangesActionFilterAttribute))]
        public async Task<int> StartAuctionAsync([FromBody] StartAuctionRequestDto request)
        {
            var command = _mapper.Map<StartAuctionCommand>(request);
            var result = await _startAuctionCommandHandler.HandleAsync(command);
            return result;
        }

        [HttpPut]
        [ServiceFilter(typeof(SaveChangesActionFilterAttribute))]
        public async Task<int> CloseAuctionAsync([FromBody] CloseAuctionRequestDto request)
        {
            var command = _mapper.Map<CloseAuctionCommand>(request);
            var result = await _closeAuctionCommandHandler.HandleAsync(command);
            return result;
        }
    }
}
