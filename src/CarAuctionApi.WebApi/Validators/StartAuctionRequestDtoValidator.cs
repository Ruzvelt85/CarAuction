using CarAuctionApi.Dto.Requests;
using FluentValidation;

namespace CarAuctionApi.WebApi.Validators
{
    public class StartAuctionRequestDtoValidator : AbstractValidator<StartAuctionRequestDto>
    {
        public StartAuctionRequestDtoValidator()
        {
            RuleFor(_ => _.VehicleId).GreaterThan(0);
        }
    }
}
