using TestVadarod.Data.Models;
using TestVadarod.Data.Interfaces.Repositories;
using TestVadarod.Data.Interfaces.Services;
using TestVadarod.Services;
using TestVadarod.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

namespace TestVadarod
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connection = builder.Configuration.GetConnectionString("Database");

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            builder.Host
             .UseSerilog((ctx, lc) => lc
             .WriteTo.Console()
             .ReadFrom.Configuration(ctx.Configuration));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var basePath = AppContext.BaseDirectory;

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddDbContext<CurrencyRateDbContext>(options => options.UseNpgsql(connection));
            builder.Services.AddHttpClient<Rate>();
            
            builder.Services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
            builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();

            var app = builder.Build();

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
