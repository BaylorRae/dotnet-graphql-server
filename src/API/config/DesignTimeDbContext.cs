using System.IO;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace API.config
{
    public class DesignTimeDbContext : IDesignTimeDbContextFactory<BicycleShopContext>
    {
        public BicycleShopContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<BicycleShopContext>();

            var connectionString = configuration.GetConnectionString("BicycleShop");

            builder.UseSqlServer(connectionString);

            return new BicycleShopContext(builder.Options);
        }
    }
}