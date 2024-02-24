using AutoFixture;
using AutoMapper;
using CarAuctionApi.Dto.Requests;
using CarAuctionApi.Dto.Responses;
using CarAuctionApi.Patterns;
using CarAuctionApi.WebApi.Commands;
using CarAuctionApi.WebApi.Controllers;
using CarAuctionApi.WebApi.Mappings;
using CarAuctionApi.WebApi.Queries;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarAuctionApi.Tests.Controllers
{
    public class InventoryControllerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IQueryHandler<GetVehicleListQuery, VehicleListResponseDto>> _getVehicleListQueryHandlerMock;
        private readonly Mock<ICommandHandler<CreateVehicleCommand, int>> _createVehicleCommandHandlerMock;

        public InventoryControllerTests()
        {
            this._mapperMock = new Mock<IMapper>();
            this._getVehicleListQueryHandlerMock = new Mock<IQueryHandler<GetVehicleListQuery, VehicleListResponseDto>>();
            this._createVehicleCommandHandlerMock = new Mock<ICommandHandler<CreateVehicleCommand, int>>();
        }

        [Fact]
        public void Constructor_WithNullMapper_ThrowsArgumentNullException()
        {
            var controller = () => new InventoryController(
                default!,
                this._getVehicleListQueryHandlerMock.Object,
                this._createVehicleCommandHandlerMock.Object);
            controller.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullQueryHandler_ThrowsArgumentNullException()
        {
            var controller = () => new InventoryController(
                this._mapperMock.Object,
                default!,
                this._createVehicleCommandHandlerMock.Object);
            controller.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullCommandHandler_ThrowsArgumentNullException()
        {
            var controller = () => new InventoryController(
                this._mapperMock.Object,
                this._getVehicleListQueryHandlerMock.Object,
                default!);
            controller.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task GetVehicleListAsync_ValidRequestDto_ReturnsOkContent()
        {
            // Arrange
            var mapper = new MapperConfiguration(cfg => cfg.AddMaps(typeof(InventoryProfile).Assembly))
                .CreateMapper();
            var targetController = new InventoryController(mapper, this._getVehicleListQueryHandlerMock.Object, this._createVehicleCommandHandlerMock.Object);
            var expectedResponse = new VehicleListResponseDto
            {
                Items = new[] { new Fixture().Build<VehicleResponseDto>().Create() },
            };
            this._getVehicleListQueryHandlerMock
                .Setup(m => m.HandleAsync(It.IsAny<GetVehicleListQuery>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var actionResult = await targetController.GetVehicleListAsync(new GetVehicleListRequestDto());

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            Assert.IsType<VehicleListResponseDto>(result!.Value);
            this._getVehicleListQueryHandlerMock.Verify(
                queryHandler => queryHandler.HandleAsync(It.IsAny<GetVehicleListQuery>()),
                Times.Once);
            this._getVehicleListQueryHandlerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddVehicleAsync_ValidRequestDto_ReturnsCorrectValue()
        {
            // Arrange
            var mapper = new MapperConfiguration(cfg => cfg.AddMaps(typeof(InventoryProfile).Assembly))
                .CreateMapper();
            var targetController = new InventoryController(mapper, this._getVehicleListQueryHandlerMock.Object, this._createVehicleCommandHandlerMock.Object);
            this._createVehicleCommandHandlerMock
                .Setup(m => m.HandleAsync(It.IsAny<CreateVehicleCommand>()))
                .ReturnsAsync(1);

            // Act
            var result = await targetController.AddVehicleAsync(new Fixture().Build<CreateVehicleRequestDto>().Create());

            // Assert
            result.Should().NotBeNull();
            Assert.IsType<int>(result);
            this._createVehicleCommandHandlerMock.Verify(
                commandHandler => commandHandler.HandleAsync(It.IsAny<CreateVehicleCommand>()),
                Times.Once);
            this._createVehicleCommandHandlerMock.VerifyNoOtherCalls();
        }
    }
}
