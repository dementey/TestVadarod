using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestVadarod.Data.Models;

namespace TestVadarod.Repositories
{
    public class CurrencyRateDbContext : DbContext
    {
        public DbSet<Rate> Rates { get; set; }

        public CurrencyRateDbContext(DbContextOptions<CurrencyRateDbContext> options)
          : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
