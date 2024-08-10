using TestVadarod.Data.Models;

namespace TestVadarod.Data.Interfaces.Repositories
{
    public interface ICurrencyRateRepository
    {
        Task<IEnumerable<Rate>> Add(DateTime date);
        Task<Rate> GetByCurrencyAndDate(int cur_id, DateTime date);
    }
}
