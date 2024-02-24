using CarAuctionApi.Dto.Requests;
using CarAuctionApi.WebApi.Validators;
using FluentValidation.TestHelper;

namespace CarAuctionApi.Tests.Validators
{
    public class CloseAuctionRequestDtoValidatorTests
    {
        private readonly CloseAuctionRequestDto _defaultModel;
        private readonly CloseAuctionRequestDtoValidator _dtoValidator;

        public CloseAuctionRequestDtoValidatorTests()
        {
            _defaultModel = new CloseAuctionRequestDto();
            _dtoValidator = new CloseAuctionRequestDtoValidator();
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
            var model = new CloseAuctionRequestDto { VehicleId = 0 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.VehicleId);
        }

        [Fact]
        public async Task VehicleId_Correct_ShouldHaveValidationError()
        {
            var model = new CloseAuctionRequestDto { VehicleId = 10 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
