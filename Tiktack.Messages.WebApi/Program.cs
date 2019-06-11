using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Tiktack.Messaging.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(x => x.ListenAnyIP(4000));
                });
    }
}
