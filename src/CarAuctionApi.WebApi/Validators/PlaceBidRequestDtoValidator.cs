using CarAuctionApi.Dto.Requests;
using FluentValidation;

namespace CarAuctionApi.WebApi.Validators
{
    public class PlaceBidRequestDtoValidator : AbstractValidator<PlaceBidRequestDto>
    {
        public PlaceBidRequestDtoValidator()
        {
            RuleFor(_ => _.VehicleId).GreaterThan(0);
            RuleFor(_ => _.CustomerId).GreaterThan(0);
            RuleFor(_ => _.Value).GreaterThan(0);
        }
    }
}
