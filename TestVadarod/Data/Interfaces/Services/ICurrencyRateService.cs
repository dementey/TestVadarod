using TestVadarod.Data.Models;

namespace TestVadarod.Data.Interfaces.Services
{
    public interface ICurrencyRateService
    {
        Task<IEnumerable<Rate>> Add(DateTime date);
        Task<Rate> GetByCurrencyAndDate(int cur_id, DateTime date);
    }
}
