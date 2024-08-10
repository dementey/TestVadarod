using TestVadarod.Data.Models;
using TestVadarod.Data.Interfaces.Repositories;
using TestVadarod.Data.Interfaces.Services;
using TestVadarod.Services;
using TestVadarod.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.File;
    
namespace TestVadarod
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connection = builder.Configuration.GetConnectionString("Database");

            // траблы с DateTime.Date.Kind postgres принимает только Utc
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            builder.Host
             .UseSerilog((ctx, lc) => lc
             .WriteTo.Console()
             .ReadFrom.Configuration(ctx.Configuration));
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<CurrencyRateDbContext>(options => options.UseNpgsql(connection));
            builder.Services.AddHttpClient<Rate>();
            
            builder.Services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
            builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
