using CarAuctionApi.Dto.Common;
using CarAuctionApi.Dto.Requests;
using CarAuctionApi.WebApi.Validators;
using FluentValidation.TestHelper;

namespace CarAuctionApi.Tests.Validators
{
    public class GetVehicleListRequestDtoValidatorTests
    {
        private readonly GetVehicleListRequestDto _defaultModel;
        private readonly GetVehicleListRequestDtoValidator _dtoValidator;

        public GetVehicleListRequestDtoValidatorTests()
        {
            _defaultModel = new GetVehicleListRequestDto();
            _dtoValidator = new GetVehicleListRequestDtoValidator();
        }

        [Fact]
        public async Task Default_ShouldNotHaveValidationError()
        {
            var result = await _dtoValidator.TestValidateAsync(_defaultModel);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task CorrectValues_ShouldNotHaveValidationError()
        {
            var model = new GetVehicleListRequestDto
            {
                Type = VehicleType.Sedan,
                Manufacturer = "Renault",
                Model = "Logan",
                Year = 2010
            };
            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Manufacturer_Empty_ShouldNotHaveValidationError()
        {
            var model = _defaultModel with { Manufacturer = "" };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Manufacturer_TooBig_ShouldHaveValidationError()
        {
            var model = _defaultModel with { Manufacturer = new string('a', 129) };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Manufacturer);
        }

        [Fact]
        public async Task Model_Empty_ShouldNotHaveValidationError()
        {
            var model = _defaultModel with { Model = "" };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Model_TooBig_ShouldHaveValidationError()
        {
            var model = _defaultModel with { Model = new string('a', 129) };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Model);
        }

        [Fact]
        public async Task Year_Default_ShouldHaveValidationError()
        {
            var model = _defaultModel with { Year = 0 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Year);
        }

        [Fact]
        public async Task Year_LessThan1850_ShouldHaveValidationError()
        {
            var model = _defaultModel with { Year = 1849 };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Year);
        }

        [Fact]
        public async Task Year_MoreThanCurrent_ShouldHaveValidationError()
        {
            var model = _defaultModel with { Year = DateTime.Now.AddYears(1).Year };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(_ => _.Year);
        }

        [Fact]
        public async Task Year_Current_ShouldNotHaveValidationError()
        {
            var model = _defaultModel with { Year = DateTime.Now.Year };

            var result = await _dtoValidator.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
