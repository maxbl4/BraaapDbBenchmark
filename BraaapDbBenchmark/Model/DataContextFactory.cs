using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BraaapDbBenchmark.Model
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var configuration = Config.Initialize(args);
            var cs = configuration.GetConnectionString("MySql");
            return Create(cs);
        }

        public static DataContext Create(string connectionString)
        {
            var options = new DbContextOptionsBuilder<DataContext>();
            options.UseMySql(connectionString);
            return new DataContext(options.Options);
        }
    }
}