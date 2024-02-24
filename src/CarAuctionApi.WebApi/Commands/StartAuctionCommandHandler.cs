using AutoMapper;
using CarAuctionApi.Data.Repositories;
using CarAuctionApi.Domain.Exceptions;
using CarAuctionApi.Domain.Model;
using CarAuctionApi.Patterns;

namespace CarAuctionApi.WebApi.Commands
{
    public class StartAuctionCommandHandler : ICommandHandler<StartAuctionCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IAuctionRepository _auctionRepository;

        public StartAuctionCommandHandler(IMapper mapper, IVehicleRepository vehicleRepository, IAuctionRepository auctionRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        }

        /// <inheritdoc />
        public async Task<int> HandleAsync(StartAuctionCommand command)
        {
            var isVehicleExists = await _vehicleRepository.ExistsAsync(vehicle => vehicle.Id == command.VehicleId);
            
            if (!isVehicleExists)
            {
                throw new NotFoundException($"The auction cannot be started: the vehicle with Id={command.VehicleId} does not exist.");
            }

            var isActiveAuctionExists = await _auctionRepository.ExistsAsync(auction => auction.VehicleId == command.VehicleId && auction.IsActive);

            if (isActiveAuctionExists)
            {
                throw new ConflictException($"The auction for the vehicle with Id={command.VehicleId} has already started.");
            }

            var auctionToStart = _mapper.Map<Auction>(command);
            var startedAuction = await _auctionRepository.AddAsync(auctionToStart);
            
            return startedAuction.Id;
        }
    }
}
