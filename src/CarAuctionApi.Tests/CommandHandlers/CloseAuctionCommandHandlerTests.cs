using System.Linq.Expressions;
using Moq;
using FluentAssertions;
using CarAuctionApi.Data.Repositories;
using CarAuctionApi.Domain.Model;
using CarAuctionApi.WebApi.Commands;
using CarAuctionApi.Domain.Exceptions;
using AutoFixture;

namespace CarAuctionApi.Tests.CommandHandlers
{
    public class CloseAuctionCommandHandlerTests
    {
        private readonly Mock<IAuctionRepository> _auctionRepositoryMock;
        private readonly Mock<IBidRepository> _bidRepositoryMock;

        public CloseAuctionCommandHandlerTests()
        {
            this._auctionRepositoryMock = new Mock<IAuctionRepository>();
            this._bidRepositoryMock = new Mock<IBidRepository>();
        }
        
        [Fact]
        public void Constructor_WithNullAuctionRepository_ThrowsArgumentNullException()
        {
            var action = () => new CloseAuctionCommandHandler(
                default!,
                this._bidRepositoryMock.Object);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullBidRepository_ThrowsArgumentNullException()
        {
            var action = () => new CloseAuctionCommandHandler(
                this._auctionRepositoryMock.Object,
                default!);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleAsync_ActiveAuctionExists_ReturnsValidResponse()
        {
            // Arrange
            var (auctionId, vehicleId) = (new Random().Next(), new Random().Next());
            var bids = new Fixture().Build<Bid>().CreateMany().ToList();
            var auction = new Fixture().Build<Auction>().With(_ => _.Id, auctionId).Create();
            this._auctionRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Auction, bool>>>()))
                .ReturnsAsync(auction);
            this._auctionRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Auction>()))
                .ReturnsAsync(auction);
            
            this._bidRepositoryMock
                .Setup(x => x.Find(It.IsAny<Expression<Func<Bid, bool>>>()))
                .Returns(bids);
            this._bidRepositoryMock
                .Setup(x => x.DeleteRangeAsync(It.IsAny<IEnumerable<Bid>>()));
            var command = new CloseAuctionCommand { VehicleId = vehicleId };

            // Act
            var handler = new CloseAuctionCommandHandler(this._auctionRepositoryMock.Object, this._bidRepositoryMock.Object);
            var result = await handler.HandleAsync(command);

            // Assert
            this._auctionRepositoryMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Auction, bool>>>()), Times.Once);
            this._auctionRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Auction>()), Times.Once);
            this._bidRepositoryMock.Verify(r => r.Find(It.IsAny<Expression<Func<Bid, bool>>>()), Times.Once);
            this._bidRepositoryMock.Verify(r => r.DeleteRangeAsync(It.IsAny<IEnumerable<Bid>>()), Times.Once);
            result.Should().Be(auctionId);
        }

        [Fact]
        public async Task HandleAsync_ActiveAuctionNotExists_ThrowsNotFoundException()
        {
            // Arrange
            this._auctionRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Auction, bool>>>()))
                .ReturnsAsync((Auction?)null);
            var command = new CloseAuctionCommand { VehicleId = new Random().Next() };

            // Act && Assert
            var handler = new CloseAuctionCommandHandler(this._auctionRepositoryMock.Object, this._bidRepositoryMock.Object);
            var action = async () => await handler.HandleAsync(command);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>();
            this._auctionRepositoryMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Auction, bool>>>()), Times.Once);
            this._auctionRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Auction>()), Times.Never);
            this._bidRepositoryMock.Verify(r => r.Find(It.IsAny<Expression<Func<Bid, bool>>>()), Times.Never);
            this._bidRepositoryMock.Verify(r => r.DeleteRangeAsync(It.IsAny<IEnumerable<Bid>>()), Times.Never);
        }
    }
}
