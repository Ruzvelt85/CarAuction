using CarAuctionApi.Data.Repositories;
using CarAuctionApi.Domain.Exceptions;
using CarAuctionApi.Patterns;

namespace CarAuctionApi.WebApi.Commands
{
    public class CloseAuctionCommandHandler : ICommandHandler<CloseAuctionCommand, int>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;

        public CloseAuctionCommandHandler(IAuctionRepository auctionRepository, IBidRepository bidRepository)
        {
            _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
            _bidRepository = bidRepository ?? throw new ArgumentNullException(nameof(bidRepository));
        }

        /// <inheritdoc />
        public async Task<int> HandleAsync(CloseAuctionCommand command)
        {
            var existingAuction = await _auctionRepository.SingleOrDefaultAsync(auction => auction.VehicleId == command.VehicleId && auction.IsActive);

            if (existingAuction == null)
            {
                throw new NotFoundException($"The active auction for the vehicle with Id={command.VehicleId} does not exist.");
            }

            existingAuction.IsActive = false;
            var closedAuction = await _auctionRepository.UpdateAsync(existingAuction);

            // Now we need to delete all the bids from the closed auction
            var bidsToRemove = _bidRepository.Find(bid => bid.VehicleId == command.VehicleId);
            if (bidsToRemove.Any())
            {
                await _bidRepository.DeleteRangeAsync(bidsToRemove);
            }

            return closedAuction.Id;
        }
    }
}
