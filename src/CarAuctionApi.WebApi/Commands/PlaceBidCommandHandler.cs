using AutoMapper;
using CarAuctionApi.Data.Repositories;
using CarAuctionApi.Domain.Exceptions;
using CarAuctionApi.Domain.Model;
using CarAuctionApi.Patterns;

namespace CarAuctionApi.WebApi.Commands
{
    public class PlaceBidCommandHandler : ICommandHandler<PlaceBidCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;

        public PlaceBidCommandHandler(IMapper mapper, IVehicleRepository vehicleRepository, IAuctionRepository auctionRepository, IBidRepository bidRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
            _bidRepository = bidRepository ?? throw new ArgumentNullException(nameof(bidRepository));
        }

        /// <inheritdoc />
        public async Task<int> HandleAsync(PlaceBidCommand command)
        {
            var isActiveAuctionExists = await _auctionRepository.ExistsAsync(auction => auction.VehicleId == command.VehicleId && auction.IsActive);
            
            if (!isActiveAuctionExists)
            {
                throw new NotFoundException($"The active auction for the vehicle with Id={command.VehicleId} does not exist.");
            }

            var existingVehicle = await _vehicleRepository.SingleOrDefaultAsync(vehicle => vehicle.Id == command.VehicleId);

            if (existingVehicle == null)
            {
                throw new NotFoundException($"The vehicle with Id={command.VehicleId} does not exist.");
            }

            var highestBidValue = GetHighestBid(existingVehicle);
            if (command.Value < highestBidValue)
            {
                throw new ValidationException($"The bid wasn't placed: there are bids with higher amount.");
            }

            var bidToPlace = _mapper.Map<Bid>(command);
            var placedBid = await _bidRepository.AddAsync(bidToPlace);
            
            return placedBid.Id;
        }

        private decimal GetHighestBid(Vehicle vehicle)
        {
            var bidValue = _bidRepository.GetMaxBid(vehicle.Id);

            if (bidValue is > 0)
            {
                return bidValue.Value;
            }

            return vehicle.StartingBid;
        }
    }
}
