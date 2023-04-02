using Dbank.Digisoft.PrediBet.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

BuildWebHost(args).Run();

IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .Build();
