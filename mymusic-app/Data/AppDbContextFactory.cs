using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using mymusic_app.Controllers.Data;

namespace mymusic_app.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Use Npgsql for PostgreSQL
            optionsBuilder.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
