using System.Linq;
using Dovecord.Shared;

namespace Dovecord.Data.Seeders
{
    public class MessageSeeder
    {
        public static void SeedSampleChannelData(ApplicationDbContext context)
        {
            if (!context.Channels.Any())
            {
                //context.Channels.Add(new AutoFaker<Channel>());
            }
        }
    }
}