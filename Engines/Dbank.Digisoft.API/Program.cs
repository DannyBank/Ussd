using Dbank.Digisoft.Api;
using Dbank.Digisoft.Ussd.SDK.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

BuildWebHost(args).Run();

IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
            .AddSerilogLogging()
            .UseStartup<Startup>()
            .Build();
