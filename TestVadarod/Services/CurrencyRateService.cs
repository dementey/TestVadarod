using TestVadarod.Data.Interfaces.Services;
using TestVadarod.Data.Interfaces.Repositories;
using TestVadarod.Data.Models;

namespace TestVadarod.Services
{
    public class CurrencyRateService : ICurrencyRateService
    {
        private readonly ICurrencyRateRepository _currencyRateRepository;

        public CurrencyRateService(ICurrencyRateRepository currencyRateRepository)
        {
            _currencyRateRepository = currencyRateRepository;
        }

        public async Task<Rate> GetByCurrencyAndDate(int cur_id, DateTime date)
        {
            return await _currencyRateRepository.GetByCurrencyAndDate(cur_id, date);
        }

        public async Task<IEnumerable<Rate>> Add(DateTime date)
        {
            return await _currencyRateRepository.Add(date);
        }
    }
}
