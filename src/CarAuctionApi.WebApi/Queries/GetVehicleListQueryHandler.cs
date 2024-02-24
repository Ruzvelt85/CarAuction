using System.Linq.Expressions;
using AutoMapper;
using CarAuctionApi.Data.Repositories;
using CarAuctionApi.Domain.Model;
using CarAuctionApi.Dto.Responses;
using CarAuctionApi.Patterns;

namespace CarAuctionApi.WebApi.Queries
{
    public class GetVehicleListQueryHandler : IQueryHandler<GetVehicleListQuery, VehicleListResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IVehicleRepository _vehicleRepository;

        public GetVehicleListQueryHandler(IMapper mapper, IVehicleRepository vehicleRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }

        public async Task<VehicleListResponseDto> HandleAsync(GetVehicleListQuery query)
        {
            var filterCondition = GetFilterCondition(query);
            var vehicles = _vehicleRepository.Find(filterCondition);
            return await Task.FromResult(_mapper.Map<VehicleListResponseDto>(vehicles));
        }

        private Expression<Func<Vehicle, bool>> GetFilterCondition(GetVehicleListQuery query)
        {
            // TODO: Can be improved by creating specifications
            return vehicle => (!query.Type.HasValue || vehicle.Type == _mapper.Map<VehicleType>(query.Type))
                              && (!query.Year.HasValue || vehicle.Year == query.Year.Value)
                              && (string.IsNullOrWhiteSpace(query.Model) || vehicle.Model == query.Model)
                              && (string.IsNullOrWhiteSpace(query.Manufacturer) || vehicle.Manufacturer == query.Manufacturer);
        }
    }
}
