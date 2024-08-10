using TestVadarod.Data.Models;
using Moq;
using Xunit;
using TestVadarod.Controllers;
using Microsoft.AspNetCore.Mvc;
using TestVadarod.Data.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace TestVadarod.Tests
{
    public class CurrencyRateControllerTests
    {
        private readonly Mock<ICurrencyRateService> _mockCurrencyRateService;
        private readonly Mock<ILogger<CurrencyRateController>> _mocklogger;
        private readonly CurrencyRateController _controller;

        public CurrencyRateControllerTests()
        {
            _mockCurrencyRateService = new Mock<ICurrencyRateService>();
            _mocklogger = new Mock<ILogger<CurrencyRateController>>();
            _controller = new CurrencyRateController(_mockCurrencyRateService.Object, _mocklogger.Object);
        }


        [Fact]
        public async Task Post_ShouldReturnOk_WhenRatesAreAdded()
        {
            // Arrange
            var date = DateTime.Now;
            _mockCurrencyRateService.Setup(service => service.Add(date))
                .ReturnsAsync(new[] { new Rate() {
                    Id = new Guid(),
                    Cur_ID = 305,
                    Date = new DateTime(2020, 12, 12),
                    Cur_Abbreviation = "CZK",
                    Cur_Name = "Чешских крон",
                    Cur_OfficialRate = 11.6601m,
                    Cur_Scale = 100 } }); 

            // Act
            var result = await _controller.Post(date);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Rates added", actionResult.Value);
        }

        [Fact]
        public async Task Post_ShouldReturnOk_WhenRatesAlreadyExist()
        {
            // Arrange
            var date = DateTime.Now;
            _mockCurrencyRateService.Setup(service => service.Add(date))
                .ReturnsAsync((Rate[])null);

            // Act
            var result = await _controller.Post(date);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Rates have already been added before", actionResult.Value);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var date = DateTime.Now;
            _mockCurrencyRateService.Setup(service => service.Add(date))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _controller.Post(date);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Get_ShouldReturnOk_WhenRateIsFound()
        {
            // Arrange
            var date = new DateTime(2020, 12, 12);
            var curId = 305;
            var rate = new Rate()
            {
                Id = new Guid(),
                Cur_ID = 305,
                Date = new DateTime(2020, 12, 12),
                Cur_Abbreviation = "CZK",
                Cur_Name = "Чешских крон",
                Cur_OfficialRate = 11.6601m,
                Cur_Scale = 100
            }; 
            _mockCurrencyRateService.Setup(service => service.GetByCurrencyAndDate(curId, date))
                .ReturnsAsync(rate);

            // Act
            var result = await _controller.Get(curId, date);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(rate, actionResult.Value);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenRateIsNotFound()
        {
            // Arrange
            var date = DateTime.Now;
            var curId = 3000;
            _mockCurrencyRateService.Setup(service => service.GetByCurrencyAndDate(curId, date))
                .ReturnsAsync((Rate)null);

            // Act
            var result = await _controller.Get(curId, date);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var date = DateTime.Now;
            var curId = 305;
            _mockCurrencyRateService.Setup(service => service.GetByCurrencyAndDate(curId, date))
                .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.Get(curId, date);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
