using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Dovecord.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
        
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .Build(); 
            
        var builder = new DbContextOptionsBuilder(); 
        var connectionString = configuration.GetConnectionString("DatabaseConnection"); 
        builder.UseSqlite("Data Source=DovecordHQ.db",
            x => x.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.FullName));
        return new ApplicationDbContext(builder.Options); 
    }
}