using AutoFixture;
using AutoMapper;
using CarAuctionApi.Dto.Requests;
using CarAuctionApi.Dto.Responses;
using CarAuctionApi.Patterns;
using CarAuctionApi.WebApi.Commands;
using CarAuctionApi.WebApi.Controllers;
using CarAuctionApi.WebApi.Mappings;
using FluentAssertions;
using Moq;

namespace CarAuctionApi.Tests.Controllers
{
    public class AuctionsControllerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICommandHandler<StartAuctionCommand, int>> _startAuctionCommandHandlerMock;
        private readonly Mock<ICommandHandler<CloseAuctionCommand, int>> _closeAuctionCommandHandlerMock;

        public AuctionsControllerTests()
        {
            this._mapperMock = new Mock<IMapper>();
            this._startAuctionCommandHandlerMock = new Mock<ICommandHandler<StartAuctionCommand, int>>();
            this._closeAuctionCommandHandlerMock = new Mock<ICommandHandler<CloseAuctionCommand, int>>();
        }

        [Fact]
        public void Constructor_WithNullMapper_ThrowsArgumentNullException()
        {
            var controller = () => new AuctionsController(
                default!,
                this._startAuctionCommandHandlerMock.Object,
                this._closeAuctionCommandHandlerMock.Object);
            controller.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullStartAuctionCommandHandler_ThrowsArgumentNullException()
        {
            var controller = () => new AuctionsController(
                this._mapperMock.Object,
                default!,
                this._closeAuctionCommandHandlerMock.Object);
            controller.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullCloseAuctionCommandHandler_ThrowsArgumentNullException()
        {
            var controller = () => new AuctionsController(
                this._mapperMock.Object,
                this._startAuctionCommandHandlerMock.Object,
                default!);
            controller.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task StartAuctionAsync_ValidRequestDto_ReturnsOkContent()
        {
            // Arrange
            var mapper = new MapperConfiguration(cfg => cfg.AddMaps(typeof(InventoryProfile).Assembly))
                .CreateMapper();
            var targetController = new AuctionsController(mapper, this._startAuctionCommandHandlerMock.Object, this._closeAuctionCommandHandlerMock.Object);
            var expectedResponse = new VehicleListResponseDto
            {
                Items = new[] { new Fixture().Build<VehicleResponseDto>().Create() },
            };
            this._startAuctionCommandHandlerMock
                .Setup(m => m.HandleAsync(It.IsAny<StartAuctionCommand>()))
                .ReturnsAsync(1);

            // Act
            var result = await targetController.StartAuctionAsync(new Fixture().Build<StartAuctionRequestDto>().Create());

            // Assert
            Assert.IsType<int>(result);
            this._startAuctionCommandHandlerMock.Verify(
                commandHandler => commandHandler.HandleAsync(It.IsAny<StartAuctionCommand>()),
                Times.Once);
            this._startAuctionCommandHandlerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CloseAuctionAsync_ValidRequestDto_ReturnsCorrectValue()
        {
            // Arrange
            var mapper = new MapperConfiguration(cfg => cfg.AddMaps(typeof(InventoryProfile).Assembly))
                .CreateMapper();
            var targetController = new AuctionsController(mapper, this._startAuctionCommandHandlerMock.Object, this._closeAuctionCommandHandlerMock.Object);
            this._closeAuctionCommandHandlerMock
                .Setup(m => m.HandleAsync(It.IsAny<CloseAuctionCommand>()))
                .ReturnsAsync(1);

            // Act
            var result = await targetController.CloseAuctionAsync(new Fixture().Build<CloseAuctionRequestDto>().Create());

            // Assert
            Assert.IsType<int>(result);
            this._closeAuctionCommandHandlerMock.Verify(
                commandHandler => commandHandler.HandleAsync(It.IsAny<CloseAuctionCommand>()),
                Times.Once);
            this._closeAuctionCommandHandlerMock.VerifyNoOtherCalls();
        }
    }
}
