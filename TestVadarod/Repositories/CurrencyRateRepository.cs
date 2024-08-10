using Microsoft.EntityFrameworkCore;
using TestVadarod.Data.Interfaces.Repositories;
using TestVadarod.Data.Models;

namespace TestVadarod.Repositories
{
    public class CurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly CurrencyRateDbContext _dbContext;
        private readonly HttpClient _httpClient;

        public CurrencyRateRepository(CurrencyRateDbContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        public async Task<Rate> GetByCurrencyAndDate(int cur_id, DateTime date)
        {
            var rate = await _dbContext.Rates
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Cur_ID == cur_id && r.Date == date);

            return rate;
        }

        public async Task<IEnumerable<Rate>> Add(DateTime date)
        {
            if (!_dbContext.Rates.Any(r => r.Date == date))
            {
                var response = await _httpClient.GetAsync($"https://www.nbrb.by/api/exrates/rates?ondate={date:yyyy-MM-dd}&periodicity=0");
                var rates = await response.Content.ReadFromJsonAsync<IEnumerable<Rate>>();

                await _dbContext.AddRangeAsync(rates);
                await _dbContext.SaveChangesAsync();

                return rates;
            }

            return null;
        }
    }
}
