using CarAuctionApi.Dto.Requests;
using CarAuctionApi.WebApi.Validators;
using FluentValidation.TestHelper;

namespace CarAuctionApi.Tests.Validators
{
    public class PlaceBidRequestDtoValidatorTests
    {
        private readonly PlaceBidRequestDto _defaultModel;
        private readonly PlaceBidRequestDtoValidator _dtoValidator;

        public PlaceBidRequestDtoValidatorTests()
        {
            _defaultModel = new PlaceBidRequestDto();
            _dtoValidator = new PlaceBidRequestDtoValidator();
        }

        [Fact]
        public async Task Default_ShouldHaveValidationError()
        {
            var result = await _dtoValidator.TestValidateAsync(_defaultModel);

            result.ShouldHaveValidationErrorFor(_ => _.VehicleId);
            result.ShouldHaveValidationErrorFor(_ => _.CustomerId);
            result.ShouldHaveValidationErrorFor(_ => _.Value);
        }

        [Fact]
        public async Task CorrectValues_ShouldNotHaveValidationError()
        {
            var model = new PlaceBidRequestDto { VehicleId = 1, CustomerId = 2, Value = 8000 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Vehicle_Equals0_ShouldHaveValidationError()
        {
            var model = new PlaceBidRequestDto { VehicleId = 0, CustomerId = 2, Value = 8000 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.VehicleId);
        }

        [Fact]
        public async Task CustomerId_Equals0_ShouldHaveValidationError()
        {
            var model = new PlaceBidRequestDto { VehicleId = 1, CustomerId = 0, Value = 8000 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.CustomerId);
        }

        [Fact]
        public async Task Value_Equals0_ShouldHaveValidationError()
        {
            var model = new PlaceBidRequestDto { VehicleId = 1, CustomerId = 2, Value = 0 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Value);
        }
    }
}
