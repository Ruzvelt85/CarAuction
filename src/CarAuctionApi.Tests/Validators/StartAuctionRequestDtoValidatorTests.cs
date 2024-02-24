using CarAuctionApi.Dto.Requests;
using CarAuctionApi.WebApi.Validators;
using FluentValidation.TestHelper;

namespace CarAuctionApi.Tests.Validators
{
    public class StartAuctionRequestDtoValidatorTests
    {
        private readonly StartAuctionRequestDto _defaultModel;
        private readonly StartAuctionRequestDtoValidator _dtoValidator;

        public StartAuctionRequestDtoValidatorTests()
        {
            _defaultModel = new StartAuctionRequestDto();
            _dtoValidator = new StartAuctionRequestDtoValidator();
        }

        [Fact]
        public async Task Default_ShouldHaveValidationError()
        {
            var result = await _dtoValidator.TestValidateAsync(_defaultModel);

            result.ShouldHaveValidationErrorFor(_ => _.VehicleId);
        }

        [Fact]
        public async Task VehicleId_Equals0_ShouldHaveValidationError()
        {
            var model = new StartAuctionRequestDto { VehicleId = 0 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.VehicleId);
        }

        [Fact]
        public async Task VehicleId_Correct_ShouldHaveValidationError()
        {
            var model = new StartAuctionRequestDto { VehicleId = 10 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
