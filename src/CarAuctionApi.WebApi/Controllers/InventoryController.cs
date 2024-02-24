using AutoMapper;
using CarAuctionApi.Dto.Requests;
using CarAuctionApi.Dto.Responses;
using CarAuctionApi.Patterns;
using CarAuctionApi.ServiceInfrastructure.Filters;
using CarAuctionApi.WebApi.Commands;
using CarAuctionApi.WebApi.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CarAuctionApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQueryHandler<GetVehicleListQuery, VehicleListResponseDto> _getVehicleListQueryHandler;
        private readonly ICommandHandler<CreateVehicleCommand, int> _createVehicleCommandHandler;

        public InventoryController(IMapper mapper, IQueryHandler<GetVehicleListQuery, VehicleListResponseDto> getVehicleListQueryHandler, ICommandHandler<CreateVehicleCommand, int> createVehicleCommandHandler)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _getVehicleListQueryHandler = getVehicleListQueryHandler ?? throw new ArgumentNullException(nameof(getVehicleListQueryHandler));
            _createVehicleCommandHandler = createVehicleCommandHandler ?? throw new ArgumentNullException(nameof(createVehicleCommandHandler));
        }

        [HttpGet]
        public async Task<ActionResult<VehicleListResponseDto>> GetVehicleListAsync([FromQuery] GetVehicleListRequestDto request)
        {
            var query = _mapper.Map<GetVehicleListQuery>(request);
            var vehicles = await _getVehicleListQueryHandler.HandleAsync(query);
            return Ok(vehicles);
        }

        [HttpPost]
        [ServiceFilter(typeof(SaveChangesActionFilterAttribute))]
        public async Task<int?> AddVehicleAsync([FromBody] CreateVehicleRequestDto request)
        {
            var command = _mapper.Map<CreateVehicleCommand>(request);
            var result = await _createVehicleCommandHandler.HandleAsync(command);
            return result;
        }
    }
}
