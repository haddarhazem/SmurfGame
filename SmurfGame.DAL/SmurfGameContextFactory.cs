using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmurfGame.DAL
{
    public class SmurfGameContextFactory : IDesignTimeDbContextFactory<SmurfGameContext>
    {
        public SmurfGameContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SmurfGameContext>();
            optionsBuilder.UseSqlServer(
                @"server=(LocalDB)\MSSQLLocalDB;Initial Catalog=SmurfGameDB;Integrated Security=true"
            );

            return new SmurfGameContext(optionsBuilder.Options);
        }
    }
}