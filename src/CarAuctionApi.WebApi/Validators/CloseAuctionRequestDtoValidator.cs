using CarAuctionApi.Dto.Requests;
using FluentValidation;

namespace CarAuctionApi.WebApi.Validators
{
    public class CloseAuctionRequestDtoValidator : AbstractValidator<CloseAuctionRequestDto>
    {
        public CloseAuctionRequestDtoValidator()
        {
            RuleFor(_ => _.VehicleId).GreaterThan(0);
        }
    }
}
