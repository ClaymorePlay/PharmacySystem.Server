using GaneshaProgramming;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PharmacySystem.Server;

namespace CodeEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>();
    }
}
