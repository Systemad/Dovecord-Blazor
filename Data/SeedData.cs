using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dovecord.Data;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
                   serviceProvider.GetRequiredService<
                       DbContextOptions<ApplicationDbContext>>()))
        {
            //if (context.Servers.Any() && context.Channels.Any() && context.Users.Any()) return;
            Console.WriteLine("database has not been seed");
        }
    }
}