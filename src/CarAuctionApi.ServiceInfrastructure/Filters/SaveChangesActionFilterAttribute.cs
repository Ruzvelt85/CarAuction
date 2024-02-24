using CarAuctionApi.Patterns;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarAuctionApi.ServiceInfrastructure.Filters
{
    /// <summary>
    /// ASP.NET asynchronous action filter for saving all data in EF context automatically.
    /// Uses instance of UnitOfWork for saving
    /// </summary>
    public class SaveChangesActionFilterAttribute : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaveChangesActionFilterAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();

            if (result.Exception == null || result.ExceptionHandled)
            {
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
