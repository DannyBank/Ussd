using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Dbank.Digisoft.Church.Ussd {
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        public static IWebHost CreateHostBuilder(string[] args)
        {
               return WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .Build();
        }
    }
}
