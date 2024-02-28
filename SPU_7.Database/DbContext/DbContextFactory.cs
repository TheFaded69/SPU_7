using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SPU_7.Database.DbContext;

public class DbContextFactory : IDbContextFactory<DataContext>
{
    public DataContext CreateDbContext()
    {
        try
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", false, true);
            var config = builder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(config.GetConnectionString("StandConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            return new DataContext(optionsBuilder.Options);
        }
        catch (Exception e)
        {
            var message = e.InnerException != null ? e.InnerException.Message : e.Message;
            return null;
        }
    }
}