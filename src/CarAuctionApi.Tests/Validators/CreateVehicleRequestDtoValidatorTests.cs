using CarAuctionApi.Dto.Common;
using CarAuctionApi.Dto.Requests;
using CarAuctionApi.WebApi.Validators;
using FluentValidation.TestHelper;

namespace CarAuctionApi.Tests.Validators
{
    public class CreateVehicleRequestDtoValidatorTests
    {
        private readonly CreateVehicleRequestDto _defaultModel;
        private readonly CreateVehicleRequestDto _correctModel;
        private readonly CreateVehicleRequestDtoValidator _dtoValidator;

        public CreateVehicleRequestDtoValidatorTests()
        {
            _defaultModel = new CreateVehicleRequestDto();
            _correctModel = new CreateVehicleRequestDto
            {
                Id = 1,
                Type = VehicleType.Sedan,
                Manufacturer = "Renault",
                Model = "Logan",
                Year = 2010,
                StartingBid = 15000,
                DoorNumber = 5
            };
            _dtoValidator = new CreateVehicleRequestDtoValidator();
        }

        [Fact]
        public async Task Default_ShouldHaveValidationError()
        {
            var result = await _dtoValidator.TestValidateAsync(_defaultModel);

            result.ShouldHaveValidationErrorFor(_ => _.Id);
            result.ShouldHaveValidationErrorFor(_ => _.Type);
            result.ShouldHaveValidationErrorFor(_ => _.Manufacturer);
            result.ShouldHaveValidationErrorFor(_ => _.Model);
            result.ShouldHaveValidationErrorFor(_ => _.Year);
            result.ShouldHaveValidationErrorFor(_ => _.StartingBid);
        }

        [Fact]
        public async Task CorrectValues_ShouldNotHaveValidationError()
        {
            var result = await _dtoValidator.TestValidateAsync(_correctModel);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Manufacturer_Empty_ShouldHaveValidationError()
        {
            var model = _correctModel with { Manufacturer = "" };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Manufacturer);
        }

        [Fact]
        public async Task Manufacturer_TooBig_ShouldHaveValidationError()
        {
            var model = _correctModel with { Manufacturer = new string('a', 129) };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Manufacturer);
        }

        [Fact]
        public async Task Model_Empty_ShouldHaveValidationError()
        {
            var model = _correctModel with { Model = "" };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Model);
        }

        [Fact]
        public async Task Model_TooBig_ShouldHaveValidationError()
        {
            var model = _correctModel with { Model = new string('a', 129) };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Model);
        }

        [Fact]
        public async Task Year_Default_ShouldHaveValidationError()
        {
            var model = _correctModel with { Year = 0 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Year);
        }

        [Fact]
        public async Task Year_LessThan1850_ShouldHaveValidationError()
        {
            var model = _correctModel with { Year = 1849 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Year);
        }

        [Fact]
        public async Task Year_MoreThanCurrent_ShouldHaveValidationError()
        {
            var model = _correctModel with { Year = DateTime.Now.AddYears(1).Year };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Year);
        }

        [Fact]
        public async Task Year_Current_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Year = DateTime.Now.Year };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task StartingBid_Default_ShouldHaveValidationError()
        {
            var model = _correctModel with { StartingBid = 0 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.StartingBid);
        }

        [Fact]
        public async Task StartingBid_TooBig_ShouldHaveValidationError()
        {
            var model = _correctModel with { StartingBid = 1000000000 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.StartingBid);
        }

        [Fact]
        public async Task Sedan_DoorNumber_Null_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Sedan, DoorNumber = null };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.DoorNumber);
        }

        [Fact]
        public async Task Sedan_DoorNumber_Equals0_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Sedan, DoorNumber = 0 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.DoorNumber);
        }

        [Fact]
        public async Task Sedan_DoorNumber_TooBig_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Sedan, DoorNumber = 10 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.DoorNumber);
        }

        [Fact]
        public async Task Sedan_DoorNumber_Correct_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Sedan, DoorNumber = 5 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Sedan_SeatNumber_NotNull_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Sedan, DoorNumber = 5, SeatNumber = 5 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.SeatNumber);
        }

        [Fact]
        public async Task Sedan_SeatNumber_Null_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Sedan, DoorNumber = 5, SeatNumber = null };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Sedan_LoadCapacity_NotNull_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Sedan, DoorNumber = 5, LoadCapacity = 500 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.LoadCapacity);
        }

        [Fact]
        public async Task Sedan_LoadCapacity_Null_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Sedan, DoorNumber = 5, LoadCapacity = null };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Hatchback_DoorNumber_Null_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Hatchback, DoorNumber = null };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.DoorNumber);
        }

        [Fact]
        public async Task Hatchback_DoorNumber_Equals0_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Hatchback, DoorNumber = 0 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.DoorNumber);
        }

        [Fact]
        public async Task Hatchback_DoorNumber_TooBig_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Hatchback, DoorNumber = 10 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.DoorNumber);
        }

        [Fact]
        public async Task Hatchback_DoorNumber_Correct_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Hatchback, DoorNumber = 5 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Hatchback_SeatNumber_NotNull_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Hatchback, DoorNumber = 5, SeatNumber = 5 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.SeatNumber);
        }

        [Fact]
        public async Task Hatchback_SeatNumber_Null_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Hatchback, DoorNumber = 5, SeatNumber = null };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Hatchback_LoadCapacity_NotNull_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Hatchback, DoorNumber = 5, LoadCapacity = 500 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.LoadCapacity);
        }

        [Fact]
        public async Task Hatchback_LoadCapacity_Null_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Hatchback, DoorNumber = 5, LoadCapacity = null };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task SUV_DoorNumber_Null_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.SUV, DoorNumber = null, SeatNumber = 5 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task SUV_DoorNumber_NotNull_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.SUV, DoorNumber = 5, SeatNumber = 5 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.DoorNumber);
        }

        [Fact]
        public async Task SUV_SeatNumber_NotNull_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.SUV, DoorNumber = null, SeatNumber = 5 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();

        }

        [Fact]
        public async Task SUV_SeatNumber_Null_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.SUV, DoorNumber = null, SeatNumber = null };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.SeatNumber);
        }

        [Fact]
        public async Task SUV_SeatNumber_TooBig_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.SUV, DoorNumber = null, SeatNumber = 11 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.SeatNumber);
        }

        [Fact]
        public async Task SUV_SeatNumber_Equals0_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.SUV, DoorNumber = null, SeatNumber = 0 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.SeatNumber);
        }

        [Fact]
        public async Task SUV_LoadCapacity_NotNull_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.SUV, DoorNumber = null, SeatNumber = 5, LoadCapacity = 500 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.LoadCapacity);
        }

        [Fact]
        public async Task SUV_LoadCapacity_Null_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.SUV, DoorNumber = null, SeatNumber = 5, LoadCapacity = null };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Truck_DoorNumber_Null_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Truck, DoorNumber = null, LoadCapacity = 3000 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Truck_DoorNumber_NotNull_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Truck, DoorNumber = 5, LoadCapacity = 3000 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.DoorNumber);
        }

        [Fact]
        public async Task Truck_SeatNumber_Null_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Truck, DoorNumber = null, SeatNumber = null, LoadCapacity = 3000 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Truck_SeatNumber_NotNull_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Truck, DoorNumber = null, SeatNumber = 5, LoadCapacity = 3000 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.SeatNumber);
        }

        [Fact]
        public async Task Truck_LoadCapacity_Null_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Truck, DoorNumber = null, SeatNumber = null, LoadCapacity = null };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.LoadCapacity);
        }

        [Fact]
        public async Task Truck_LoadCapacity_Equals0_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Truck, DoorNumber = null, SeatNumber = null, LoadCapacity = 0 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.LoadCapacity);
        }

        [Fact]
        public async Task Truck_LoadCapacity_TooBig_ShouldHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Truck, DoorNumber = null, SeatNumber = null, LoadCapacity = 1000000 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.LoadCapacity);
        }

        [Fact]
        public async Task Truck_LoadCapacity_Correct_ShouldNotHaveValidationError()
        {
            var model = _correctModel with { Type = VehicleType.Truck, DoorNumber = null, SeatNumber = null, LoadCapacity = 500 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
