using System.Linq.Expressions;
using Moq;
using FluentAssertions;
using CarAuctionApi.Data.Repositories;
using CarAuctionApi.Domain.Model;
using CarAuctionApi.WebApi.Commands;
using CarAuctionApi.Domain.Exceptions;
using AutoFixture;
using AutoMapper;
using CarAuctionApi.WebApi.Mappings;

namespace CarAuctionApi.Tests.CommandHandlers
{
    public class PlaceBidCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        private readonly Mock<IAuctionRepository> _auctionRepositoryMock;
        private readonly Mock<IBidRepository> _bidRepositoryMock;

        public PlaceBidCommandHandlerTests()
        {
            this._mapper = new MapperConfiguration(cfg => cfg.AddMaps(typeof(InventoryProfile).Assembly)).CreateMapper();
            this._vehicleRepositoryMock = new Mock<IVehicleRepository>();
            this._auctionRepositoryMock = new Mock<IAuctionRepository>();
            this._bidRepositoryMock = new Mock<IBidRepository>();
        }

        [Fact]
        public void Constructor_WithNullMapper_ThrowsArgumentNullException()
        {
            var action = () => new PlaceBidCommandHandler(
                default!,
                this._vehicleRepositoryMock.Object,
                this._auctionRepositoryMock.Object,
                this._bidRepositoryMock.Object);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullVehicleRepository_ThrowsArgumentNullException()
        {
            var action = () => new PlaceBidCommandHandler(
                _mapper,
                default!,
                this._auctionRepositoryMock.Object,
                this._bidRepositoryMock.Object);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullAuctionRepository_ThrowsArgumentNullException()
        {
            var action = () => new PlaceBidCommandHandler(
                _mapper,
                this._vehicleRepositoryMock.Object,
                default!,
                this._bidRepositoryMock.Object);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullBidRepository_ThrowsArgumentNullException()
        {
            var action = () => new PlaceBidCommandHandler(
                _mapper,
                this._vehicleRepositoryMock.Object,
                this._auctionRepositoryMock.Object,
                default!);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleAsync_ActiveAuctionExistsAndVehicleExistsAndBidIsCorrectAndOtherBidsExist_ReturnsValidResponse()
        {
            // Arrange
            var vehicle = new Fixture().Build<Vehicle>().With(_ => _.StartingBid, 30).Create();
            var bidRoCreate = new Fixture().Build<Bid>()
                .With(_ => _.Value, 100)
                .With(_ => _.VehicleId, vehicle.Id)
                .Create();

            this._auctionRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()))
                .ReturnsAsync(true);
            this._vehicleRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .ReturnsAsync(vehicle);
            this._bidRepositoryMock
                .Setup(x => x.GetMaxBid(It.IsAny<int>()))
                .Returns(50);
            this._bidRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Bid>()))
                .ReturnsAsync(bidRoCreate);
            var command = new PlaceBidCommand
            {
                CustomerId = bidRoCreate.CustomerId, Value = bidRoCreate.Value, VehicleId = bidRoCreate.VehicleId
            };

            // Act
            var handler = new PlaceBidCommandHandler(_mapper, this._vehicleRepositoryMock.Object, this._auctionRepositoryMock.Object, this._bidRepositoryMock.Object);
            var result = await handler.HandleAsync(command);

            // Assert
            this._auctionRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()), Times.Once);
            this._vehicleRepositoryMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            this._bidRepositoryMock.Verify(r => r.GetMaxBid(It.IsAny<int>()), Times.Once);
            this._bidRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Bid>()), Times.Once);
            result.Should().Be(bidRoCreate.Id);
        }

        [Fact]
        public async Task HandleAsync_ActiveAuctionExistsAndVehicleExistsAndBidIsCorrectAndOtherBidsNotExist_ReturnsValidResponse()
        {
            // Arrange
            var vehicle = new Fixture().Build<Vehicle>().With(_ => _.StartingBid, 30).Create();
            var bidRoCreate = new Fixture().Build<Bid>()
                .With(_ => _.Value, 100)
                .With(_ => _.VehicleId, vehicle.Id)
                .Create();

            this._auctionRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()))
                .ReturnsAsync(true);
            this._vehicleRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .ReturnsAsync(vehicle);
            this._bidRepositoryMock
                .Setup(x => x.GetMaxBid(It.IsAny<int>()))
                .Returns((int?)null);
            this._bidRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Bid>()))
                .ReturnsAsync(bidRoCreate);
            var command = new PlaceBidCommand
            {
                CustomerId = bidRoCreate.CustomerId,
                Value = bidRoCreate.Value,
                VehicleId = bidRoCreate.VehicleId
            };

            // Act
            var handler = new PlaceBidCommandHandler(_mapper, this._vehicleRepositoryMock.Object, this._auctionRepositoryMock.Object, this._bidRepositoryMock.Object);
            var result = await handler.HandleAsync(command);

            // Assert
            this._auctionRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()), Times.Once);
            this._vehicleRepositoryMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            this._bidRepositoryMock.Verify(r => r.GetMaxBid(It.IsAny<int>()), Times.Once);
            this._bidRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Bid>()), Times.Once);
            result.Should().Be(bidRoCreate.Id);
        }

        [Fact]
        public async Task HandleAsync_BidValueIsLow_ThrowsValidationException()
        {
            // Arrange
            var vehicle = new Fixture().Build<Vehicle>().With(_ => _.StartingBid, 30).Create();
            var bidRoCreate = new Fixture().Build<Bid>()
                .With(_ => _.Value, 40)
                .With(_ => _.VehicleId, vehicle.Id)
                .Create();

            this._auctionRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()))
                .ReturnsAsync(true);
            this._vehicleRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .ReturnsAsync(vehicle);
            this._bidRepositoryMock
                .Setup(x => x.GetMaxBid(It.IsAny<int>()))
                .Returns(50);
            this._bidRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Bid>()))
                .ReturnsAsync(bidRoCreate);
            var command = new PlaceBidCommand
            {
                CustomerId = bidRoCreate.CustomerId,
                Value = bidRoCreate.Value,
                VehicleId = bidRoCreate.VehicleId
            };

            // Act
            var handler = new PlaceBidCommandHandler(_mapper, this._vehicleRepositoryMock.Object, this._auctionRepositoryMock.Object, this._bidRepositoryMock.Object);
            var action = async () => await handler.HandleAsync(command);

            // Assert
            await action.Should().ThrowAsync<ValidationException>();
            this._auctionRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()), Times.Once);
            this._vehicleRepositoryMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            this._bidRepositoryMock.Verify(r => r.GetMaxBid(It.IsAny<int>()), Times.Once);
            this._bidRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Bid>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_ActiveAuctionNotExists_ThrowsNotFoundException()
        {
            // Arrange
            var vehicle = new Fixture().Build<Vehicle>().With(_ => _.StartingBid, 30).Create();
            var bidRoCreate = new Fixture().Build<Bid>()
                .With(_ => _.Value, 100)
                .With(_ => _.VehicleId, vehicle.Id)
                .Create();

            this._auctionRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()))
                .ReturnsAsync(false);
            this._vehicleRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .ReturnsAsync(vehicle);
            this._bidRepositoryMock
                .Setup(x => x.GetMaxBid(It.IsAny<int>()))
                .Returns(50);
            this._bidRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Bid>()))
                .ReturnsAsync(bidRoCreate);
            var command = new PlaceBidCommand
            {
                CustomerId = bidRoCreate.CustomerId,
                Value = bidRoCreate.Value,
                VehicleId = bidRoCreate.VehicleId
            };

            // Act
            var handler = new PlaceBidCommandHandler(_mapper, this._vehicleRepositoryMock.Object, this._auctionRepositoryMock.Object, this._bidRepositoryMock.Object);
            var action = async () => await handler.HandleAsync(command);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>();
            this._auctionRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()), Times.Once);
            this._vehicleRepositoryMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Never);
            this._bidRepositoryMock.Verify(r => r.GetMaxBid(It.IsAny<int>()), Times.Never);
            this._bidRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Bid>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_VehicleNotExists_ThrowsNotFoundException()
        {
            // Arrange
            var vehicle = new Fixture().Build<Vehicle>().With(_ => _.StartingBid, 30).Create();
            var bidRoCreate = new Fixture().Build<Bid>()
                .With(_ => _.Value, 100)
                .With(_ => _.VehicleId, vehicle.Id)
                .Create();

            this._auctionRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()))
                .ReturnsAsync(true);
            this._vehicleRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .ReturnsAsync((Vehicle?) null);
            this._bidRepositoryMock
                .Setup(x => x.GetMaxBid(It.IsAny<int>()))
                .Returns(50);
            this._bidRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Bid>()))
                .ReturnsAsync(bidRoCreate);
            var command = new PlaceBidCommand
            {
                CustomerId = bidRoCreate.CustomerId,
                Value = bidRoCreate.Value,
                VehicleId = bidRoCreate.VehicleId
            };

            // Act
            var handler = new PlaceBidCommandHandler(_mapper, this._vehicleRepositoryMock.Object, this._auctionRepositoryMock.Object, this._bidRepositoryMock.Object);
            var action = async () => await handler.HandleAsync(command);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>();
            this._auctionRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Auction, bool>>>()), Times.Once);
            this._vehicleRepositoryMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            this._bidRepositoryMock.Verify(r => r.GetMaxBid(It.IsAny<int>()), Times.Never);
            this._bidRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Bid>()), Times.Never);
        }
    }
}
