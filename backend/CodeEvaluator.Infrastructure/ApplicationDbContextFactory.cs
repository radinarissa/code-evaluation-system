using CodeEvaluator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CodeEvaluator.Infrastructure
{
    public class ApplicationDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=codeevaluator;Username=postgres;Password=radi");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}