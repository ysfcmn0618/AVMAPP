using AVMAPP.Data.Infrastructure.AVMDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;

namespace AVMAPP.Data.Infrastructure.Factories
{
    public class AVMAppDbContextFactory : IDesignTimeDbContextFactory<AVMAppDbContext>
    {
        public AVMAppDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            // Modern approach: SetBasePath yerine Directory.GetCurrentDirectory() ile builder oluştur
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AVMAppDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new AVMAppDbContext(optionsBuilder.Options);
        }
    }
}



