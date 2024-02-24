using System.Linq.Expressions;
using AutoMapper;
using Moq;
using FluentAssertions;
using CarAuctionApi.Data.Repositories;
using CarAuctionApi.WebApi.Mappings;
using CarAuctionApi.Domain.Model;
using CarAuctionApi.WebApi.Commands;
using CarAuctionApi.Domain.Exceptions;

namespace CarAuctionApi.Tests.CommandHandlers
{
    public class StartAuctionCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        private readonly Mock<IAuctionRepository> _auctionRepositoryMock;

        public StartAuctionCommandHandlerTests()
        {
            this._mapper = new MapperConfiguration(cfg => cfg.AddMaps(typeof(InventoryProfile).Assembly)).CreateMapper();
            this._vehicleRepositoryMock = new Mock<IVehicleRepository>();
            this._auctionRepositoryMock = new Mock<IAuctionRepository>();
        }

        [Fact]
        public void Constructor_WithNullMapper_ThrowsArgumentNullException()
        {
            var action = () => new StartAuctionCommandHandler(
                default!,
                this._vehicleRepositoryMock.Object,
                this._auctionRepositoryMock.Object);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullVehicleRepository_ThrowsArgumentNullException()
        {
            var action = () => new StartAuctionCommandHandler(
                _mapper,
                default!,
                this._auctionRepositoryMock.Object);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullAuctionRepository_ThrowsArgumentNullException()
        {
            var action = () => new StartAuctionCommandHandler(
                _mapper,
                this._vehicleRepositoryMock.Object,
                default!);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleAsync_VehicleExistsAndNoActiveAuctionExist_ReturnsValidResponse()
        {
            // Arrange
            var (auctionId, vehicleId) = (new Random().Next(), new Random().Next());
            var command = new StartAuctionCommand { VehicleId = vehicleId };
            this._vehicleRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .ReturnsAsync(true);
            this._auctionRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()))
                .ReturnsAsync(false);
            this._auctionRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Auction>()))
                .ReturnsAsync(new Auction { Id = auctionId });

            // Act
            var handler = new StartAuctionCommandHandler(_mapper, this._vehicleRepositoryMock.Object, this._auctionRepositoryMock.Object);
            var result = await handler.HandleAsync(command);

            // Assert
            this._vehicleRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            this._auctionRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()), Times.Once);
            this._auctionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Auction>()), Times.Once);
            result.Should().Be(auctionId);
        }

        [Fact]
        public async Task HandleAsync_VehicleNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var command = new StartAuctionCommand { VehicleId = new Random().Next() };
            this._vehicleRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .ReturnsAsync(false);
            this._auctionRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()))
                .ReturnsAsync(false);
            this._auctionRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Auction>()))
                .ReturnsAsync(new Auction { Id = new Random().Next() });

            // Act && Assert
            var handler = new StartAuctionCommandHandler(_mapper, this._vehicleRepositoryMock.Object, this._auctionRepositoryMock.Object);
            var action = async () => await handler.HandleAsync(command);

            await action.Should().ThrowAsync<NotFoundException>();
            this._vehicleRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            this._auctionRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()), Times.Never);
            this._auctionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Auction>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_AuctionExists_ThrowsConflictException()
        {
            // Arrange
            var command = new StartAuctionCommand { VehicleId = new Random().Next() };
            this._vehicleRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .ReturnsAsync(true);
            this._auctionRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()))
                .ReturnsAsync(true);
            this._auctionRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Auction>()))
                .ReturnsAsync(new Auction { Id = new Random().Next() });

            // Act && Assert
            var handler = new StartAuctionCommandHandler(_mapper, this._vehicleRepositoryMock.Object, this._auctionRepositoryMock.Object);
            var action = async () => await handler.HandleAsync(command);

            await action.Should().ThrowAsync<ConflictException>();
            this._vehicleRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            this._auctionRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()), Times.Once);
            this._auctionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Auction>()), Times.Never);
        }
    }
}
