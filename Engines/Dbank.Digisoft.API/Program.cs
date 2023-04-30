using Dbank.Digisoft.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Dbank.Digisoft.Ussd.SDK.Extensions;
using Serilog;

BuildWebHost(args).Run();

IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
            .AddSerilogLogging()
            .UseStartup<Startup>()
            .Build();
