using AutoFixture;
using AutoMapper;
using CarAuctionApi.Dto.Requests;
using CarAuctionApi.Patterns;
using CarAuctionApi.WebApi.Commands;
using CarAuctionApi.WebApi.Controllers;
using CarAuctionApi.WebApi.Mappings;
using FluentAssertions;
using Moq;

namespace CarAuctionApi.Tests.Controllers
{
    public class BidsControllerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICommandHandler<PlaceBidCommand, int>> _placeBidCommandHandlerMock;

        public BidsControllerTests()
        {
            this._mapperMock = new Mock<IMapper>();
            this._placeBidCommandHandlerMock = new Mock<ICommandHandler<PlaceBidCommand, int>>();
        }

        [Fact]
        public void Constructor_WithNullMapper_ThrowsArgumentNullException()
        {
            var controller = () => new BidsController(
                default!,
                this._placeBidCommandHandlerMock.Object);
            controller.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullCommandHandler_ThrowsArgumentNullException()
        {
            var controller = () => new BidsController(
                this._mapperMock.Object,
                default!);
            controller.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task PlaceBidAsync_ValidRequestDto_ReturnsOkContent()
        {
            // Arrange
            var mapper = new MapperConfiguration(cfg => cfg.AddMaps(typeof(InventoryProfile).Assembly))
                .CreateMapper();
            var targetController = new BidsController(mapper, this._placeBidCommandHandlerMock.Object);
            this._placeBidCommandHandlerMock
                .Setup(m => m.HandleAsync(It.IsAny<PlaceBidCommand>()))
                .ReturnsAsync(1);

            // Act
            var result = await targetController.PlaceBidAsync(new Fixture().Build<PlaceBidRequestDto>().Create());

            // Assert
            Assert.IsType<int>(result);
            this._placeBidCommandHandlerMock.Verify(
                commandHandler => commandHandler.HandleAsync(It.IsAny<PlaceBidCommand>()),
                Times.Once);
            this._placeBidCommandHandlerMock.VerifyNoOtherCalls();
        }
    }
}
