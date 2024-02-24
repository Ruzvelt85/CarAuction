using System.Linq.Expressions;
using AutoMapper;
using Moq;
using FluentAssertions;
using AutoFixture;
using CarAuctionApi.Data.Repositories;
using CarAuctionApi.WebApi.Mappings;
using CarAuctionApi.Domain.Model;
using CarAuctionApi.WebApi.Commands;
using CarAuctionApi.Domain.Exceptions;

namespace CarAuctionApi.Tests.CommandHandlers
{
    public class CreateVehicleCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        
        public CreateVehicleCommandHandlerTests()
        {
            this._mapper = new MapperConfiguration(cfg => cfg.AddMaps(typeof(InventoryProfile).Assembly)).CreateMapper();
            ;
            this._vehicleRepositoryMock = new Mock<IVehicleRepository>();
        }

        [Fact]
        public void Constructor_WithNullMapper_ThrowsArgumentNullException()
        {
            var action = () => new CreateVehicleCommandHandler(
                default!,
                this._vehicleRepositoryMock.Object);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullVehicleRepository_ThrowsArgumentNullException()
        {
            var action = () => new CreateVehicleCommandHandler(
                _mapper,
                default!);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleAsync_NoVehicleExists_ReturnsValidResponse()
        {
            // Arrange
            var command = new Fixture().Build<CreateVehicleCommand>().Create();
            this._vehicleRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .ReturnsAsync(false);
            this._vehicleRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Vehicle>()))
                .ReturnsAsync(new Vehicle { Id = command.Id });

            // Act
            var handler = new CreateVehicleCommandHandler(_mapper, this._vehicleRepositoryMock.Object);
            var result = await handler.HandleAsync(command);

            // Assert
            this._vehicleRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            this._vehicleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Once);
            result.Should().Be(command.Id);
        }

        [Fact]
        public async Task HandleAsync_VehicleExists_ThrowsConflictException()
        {
            // Arrange
            var command = new Fixture().Build<CreateVehicleCommand>().Create();
            this._vehicleRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .ReturnsAsync(true);

            // Act && Assert
            var handler = new CreateVehicleCommandHandler(_mapper, this._vehicleRepositoryMock.Object);
            var action = async () => await handler.HandleAsync(command);

            await action.Should().ThrowAsync<ConflictException>();
            this._vehicleRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            this._vehicleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Never);
        }
    }
}
