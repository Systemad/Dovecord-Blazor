using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Dovecord.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
        
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        /*
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("/../Server/appsettings.json", true)
            .Build(); 
            */
        IConfigurationRoot configuration = 
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Server/appsettings.json")
                .Build();
        
        var builder = new DbContextOptionsBuilder(); 
        var connectionString = configuration.GetConnectionString("DatabaseConnection");
        builder.UseSqlite(connectionString);
            //x => x.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.FullName));
        return new ApplicationDbContext(builder.Options); 
    }
}