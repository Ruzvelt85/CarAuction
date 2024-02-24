using CarAuctionApi.Dto.Common;
using CarAuctionApi.Dto.Requests;
using FluentValidation;

namespace CarAuctionApi.WebApi.Validators
{
    public class CreateVehicleRequestDtoValidator : AbstractValidator<CreateVehicleRequestDto>
    {
        public CreateVehicleRequestDtoValidator()
        {
            RuleFor(_ => _.Id).NotNull().GreaterThan(0);
            RuleFor(_ => _.Type).IsInEnum();
            RuleFor(_ => _.Manufacturer).NotEmpty().MaximumLength(128);
            RuleFor(_ => _.Model).NotEmpty().MaximumLength(128);
            RuleFor(_ => _.Year).NotNull().GreaterThan(1850).LessThanOrEqualTo(DateTime.Today.Year);
            RuleFor(_ => _.StartingBid).NotNull().GreaterThan(0).LessThan(1000000000);
            RuleFor(_ => _.DoorNumber).NotNull().GreaterThan(0).LessThan(10).When(vehicle =>
                vehicle.Type == VehicleType.Sedan || vehicle.Type == VehicleType.Hatchback);
            RuleFor(_ => _.DoorNumber).Null().When(vehicle =>
                vehicle.Type == VehicleType.SUV || vehicle.Type == VehicleType.Truck);
            RuleFor(_ => _.SeatNumber).NotNull().GreaterThan(0).LessThan(10).When(vehicle => vehicle.Type == VehicleType.SUV);
            RuleFor(_ => _.SeatNumber).Null().When(vehicle => vehicle.Type != VehicleType.SUV);
            RuleFor(_ => _.LoadCapacity).NotNull().GreaterThan(0).LessThan(1000000).When(vehicle => vehicle.Type == VehicleType.Truck);
            RuleFor(_ => _.LoadCapacity).Null().When(vehicle => vehicle.Type != VehicleType.Truck);
        }
    }
}
