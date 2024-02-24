using CarAuctionApi.Dto.Requests;
using FluentValidation;

namespace CarAuctionApi.WebApi.Validators
{
    public class GetVehicleListRequestDtoValidator : AbstractValidator<GetVehicleListRequestDto>
    {
        public GetVehicleListRequestDtoValidator()
        {
            RuleFor(_ => _.Type).IsInEnum();
            RuleFor(_ => _.Manufacturer).MaximumLength(128);
            RuleFor(_ => _.Model).MaximumLength(128);
            RuleFor(_ => _.Year).GreaterThan(1850).LessThanOrEqualTo(DateTime.Today.Year);
        }
    }
}
