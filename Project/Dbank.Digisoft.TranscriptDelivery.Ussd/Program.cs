using Dbank.Digisoft.Ussd.SDK.Extensions;
using Microsoft.AspNetCore;

namespace Dbank.Digisoft.TranscriptDelivery.Ussd {
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
                            .UseStartup<Startup>()
                            .Build();
        }
    }
}
