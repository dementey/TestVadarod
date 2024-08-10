using Microsoft.EntityFrameworkCore;
using Moq;
using TestVadarod.Data.Models;
using TestVadarod.Repositories;
using Xunit;

namespace TestVadarod.Tests
{
    public class CurrencyRateRepositoryTests
    {
        private readonly DbContextOptions<CurrencyRateDbContext> _dbContextOptions;
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly CurrencyRateRepository _repository;

        public CurrencyRateRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<CurrencyRateDbContext>()
                .UseInMemoryDatabase(databaseName: "CurrencyRate")
                .Options;

            _mockHttpClient = new Mock<HttpClient>();

            var context = new CurrencyRateDbContext(_dbContextOptions);
            _repository = new CurrencyRateRepository(context, _mockHttpClient.Object);
        }

        [Fact]
        public async Task GetByCurrencyAndDate_ShouldReturnRate_WhenRateExists()
        {
            // Arrange
            var date = new DateTime(2020, 12, 12);
            var rate = new Rate
            {
                Id = Guid.NewGuid(),
                Cur_ID = 305,
                Date = date,
                Cur_Abbreviation = "CZK",
                Cur_Name = "Чешских крон",
                Cur_OfficialRate = 11.6601m,
                Cur_Scale = 100
            };

            using (var context = new CurrencyRateDbContext(_dbContextOptions))
            {
                await context.Rates.AddAsync(rate);
                await context.SaveChangesAsync();
            }

            // Act
            var result = await _repository.GetByCurrencyAndDate(305, date);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(305, result.Cur_ID);
            Assert.Equal(date, result.Date);
        }

        [Fact]
        public async Task GetByCurrencyAndDate_ShouldReturnNull_WhenRateDoesNotExist()
        {
            // Arrange
            var date = new DateTime(2020, 12, 12);
            var curId = 300001;

            // Act
            var result = await _repository.GetByCurrencyAndDate(curId, date);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByCurrencyAndDate_ShouldReturnNull_WhenDateDoesNotExist()
        {
            // Arrange
            var date = new DateTime(0001, 01, 01);
            var curId = 300001;

            // Act
            var result = await _repository.GetByCurrencyAndDate(curId, date);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Add_ShouldReturnNull_WhenRatesAlreadyExistForGivenDate()
        {
            // Arrange
            var date = new DateTime(2020, 12, 12);
            var rate = new Rate
            {
                Id = Guid.NewGuid(),
                Cur_ID = 305,
                Date = date,
                Cur_Abbreviation = "CZK",
                Cur_Name = "Чешских крон",
                Cur_OfficialRate = 11.6601m,
                Cur_Scale = 100
            };

            using (var context = new CurrencyRateDbContext(_dbContextOptions))
            {
                await context.Rates.AddAsync(rate);
                await context.SaveChangesAsync();
            }

            // Act
            var result = await _repository.Add(date);

            // Assert
            Assert.Null(result);

            using (var context = new CurrencyRateDbContext(_dbContextOptions))
            {
                var savedRates = await context.Rates.Where(r => r.Date == date).ToListAsync();
                Assert.Single(savedRates);
            }
        }
    }
}
