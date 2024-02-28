using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SPU_7.Database.DbContext;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        // Used only for EF .NET Core CLI tools (update database/migrations etc.)
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appsettings.json", false, true);
        var config = builder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
            .UseSqlServer(config.GetConnectionString("StandConnection"))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        return new DataContext(optionsBuilder.Options);
    }
}