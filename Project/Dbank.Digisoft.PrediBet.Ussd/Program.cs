using Dbank.Digisoft.Ussd.SDK.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Dbank.Digisoft.PrediBet.Ussd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        public static IWebHost CreateHostBuilder(string[] args)
        {           
               return WebHost.CreateDefaultBuilder(args)
                            .AddSerilogLogging()
                            .AddCustomKeyValues()
                            .UseStartup<Startup>()
                            .Build();
        }
    }
}
