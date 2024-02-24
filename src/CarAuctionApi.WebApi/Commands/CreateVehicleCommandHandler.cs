using AutoMapper;
using CarAuctionApi.Data.Repositories;
using CarAuctionApi.Domain.Exceptions;
using CarAuctionApi.Domain.Model;
using CarAuctionApi.Patterns;

namespace CarAuctionApi.WebApi.Commands
{
    public class CreateVehicleCommandHandler : ICommandHandler<CreateVehicleCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IVehicleRepository _vehicleRepository;

        public CreateVehicleCommandHandler(IMapper mapper, IVehicleRepository vehicleRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }

        /// <inheritdoc />
        public async Task<int> HandleAsync(CreateVehicleCommand command)
        {
            var isVehicleExists = await _vehicleRepository.ExistsAsync(vehicle => vehicle.Id == command.Id);
            
            if (isVehicleExists)
            {
                throw new ConflictException($"The vehicle with Id={command.Id} already exists.");
            }
            
            var vehicleToCreate = _mapper.Map<Vehicle>(command);
            var createdVehicle = await _vehicleRepository.AddAsync(vehicleToCreate);
            
            return createdVehicle.Id;
        }
    }
}
