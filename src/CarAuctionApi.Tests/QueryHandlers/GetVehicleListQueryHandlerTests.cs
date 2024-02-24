using System.Linq.Expressions;
using AutoMapper;
using Moq;
using FluentAssertions;
using AutoFixture;
using CarAuctionApi.Data.Repositories;
using CarAuctionApi.WebApi.Mappings;
using CarAuctionApi.WebApi.Queries;
using CarAuctionApi.Domain.Model;

namespace CarAuctionApi.Tests.QueryHandlers
{
    public class CreateVehicleCommandHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        
        public CreateVehicleCommandHandlerTests()
        {
            this._mapperMock = new Mock<IMapper>();
            this._vehicleRepositoryMock = new Mock<IVehicleRepository>();
        }

        [Fact]
        public void Constructor_WithNullMapper_ThrowsArgumentNullException()
        {
            var action = () => new GetVehicleListQueryHandler(
                default!,
                this._vehicleRepositoryMock.Object);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullVehicleRepository_ThrowsArgumentNullException()
        {
            var action = () => new GetVehicleListQueryHandler(
                _mapperMock.Object,
                default!);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleAsync_NoFilters_ReturnsValidResponse()
        {
            // Arrange
            var expectedVehicles = new Fixture().Build<Vehicle>().CreateMany().ToList();

            this._vehicleRepositoryMock
                .Setup(x => x.Find(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .Returns(expectedVehicles);

            // Act
            var result = await this.GetTarget() .HandleAsync(new GetVehicleListQuery());

            // Assert
            this._vehicleRepositoryMock.Verify(r => r.Find(It.IsAny<Expression<Func<Vehicle, bool>>>()));
            result.Items.Count.Should().Be(expectedVehicles.Count);
        }

        [Fact]
        public async Task HandleAsync_WithFilters_ReturnsValidResponse()
        {
            // Arrange
            var vehicles = new Fixture().Build<Vehicle>().CreateMany().ToList();
            var vehicle = vehicles.First();
            var expectedVehicles = vehicles.Where(v =>
                v.Manufacturer == vehicle.Manufacturer && v.Model == vehicle.Model && v.Year == vehicle.Year).ToList();

            this._vehicleRepositoryMock
                .Setup(x => x.Find(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .Returns(expectedVehicles);
            var query = new GetVehicleListQuery
            {
                Manufacturer = vehicle.Manufacturer,
                Model = vehicle.Model,
                Year = vehicle.Year,
            };

            // Act
            var result = await this.GetTarget().HandleAsync(query);

            // Assert
            this._vehicleRepositoryMock.Verify(r => r.Find(It.IsAny<Expression<Func<Vehicle, bool>>>()));
            result.Items.Count.Should().Be(expectedVehicles.Count);
        }

        private GetVehicleListQueryHandler GetTarget()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddMaps(typeof(InventoryProfile).Assembly)).CreateMapper();

            return new GetVehicleListQueryHandler(mapper, this._vehicleRepositoryMock.Object);
        }
    }
}
