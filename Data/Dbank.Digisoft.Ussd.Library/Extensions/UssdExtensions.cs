using Dbank.Digisoft.Ussd.SDK.Models;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.SDK.Extensions;

public static class UssdExtensions
{
    public static IWebHostBuilder AddSerilogLogging(this IWebHostBuilder webHost)
    =>
#pragma warning disable CS0618
        webHost.UseSerilog((hostingContext, loggerConfig) => loggerConfig
                                            .ReadFrom.Configuration(hostingContext.Configuration)
                                            .Enrich.FromLogContext(), writeToProviders: true);
#pragma warning restore CS0618


    public static IHostBuilder AddSerilogLogging(this IHostBuilder host)
    {
        return host.UseSerilog((context, configuration) =>
        {
            if (!context.HostingEnvironment.IsProduction())
                Serilog.Debugging.SelfLog.Enable(msg =>
                {
                    Console.WriteLine($"Serilog:: {msg}");
                    Debug.WriteLine(msg);
                });
            configuration.ReadFrom.Configuration(context.Configuration.GetSection("Logging"))
                .Enrich.FromLogContext()
                .Enrich.WithProperty("App", context.Configuration.GetValue<string>("App:AppId"))
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Destructure.ByTransformingWhere(c => c.Name == "StatusCode", (int k) => k.ToString())
                .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDestructuringDepth(3)
                    .WithRootName("ErrorDetails"));
        });
    }

    public static UssdMenu CreateUssdMenu(this MenuCollection menuItems) => menuItems.CreateUssdMenu(",");

    public static UssdMenu CreateUssdMenu(this MenuCollection menuitems, string seperator) =>
        menuitems.CreateUssdMenu(seperator, 1, AppConstants.DefaultMaxPage);

    public static UssdMenu CreateUssdMenu(this MenuCollection menuitems, string seperator, int currentPage,
        int maxPerPage)
    {
        var menuStrings = menuitems.MenuItems
            .OrderBy(c => c.Position)
            .Select(d => d.Text ?? string.Empty)
            .ToList();

        var ussdMenu = new UssdMenu(menuStrings.AsEnumerable(), menuitems.Count > maxPerPage * currentPage,
            currentPage > 1)
        {
            Header = menuitems.Header,
            Seperator = seperator,
            ExpectInput = menuitems.RequiresInput
        };

        return ussdMenu;
    }

    public static List<string> GetAllowedInputs(this MenuCollection menuitems, int maxPerPage,
        bool includeNext = false, bool includeBack = false, int page = 1, string allInputCharacter = "`")
    {
        var output = new List<string>();
        if (menuitems.RequiresInput)
        {
            if (menuitems.Count > 0)
            {
                output.AddRange(Enumerable.Range(1, menuitems.Count).Select(c => c.ToString()).ToList());
            }
            else
            {
                output.Add(allInputCharacter);
            }
        }

        return output;
    }

    public static IWebHostBuilder AddCustomKeyValues(this IWebHostBuilder builder)
    {
        try
        {
            var app = Environment.GetEnvironmentVariable("APPNAME");
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var json = GetJsonConfig(app!,env!).Result;
            return builder.ConfigureAppConfiguration(
                (WebHostBuilderContext ctx, IConfigurationBuilder config) => { 
                    config.AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(json!)));
                });
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
            return builder;
        }
    }

    private static async Task<string?> GetJsonConfig(string app, string env)
    {
        try
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (env == null) throw new ArgumentNullException(nameof(app));

            using var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5266")
            };
            var response = await httpClient.GetAsync(new Uri($"api/kv/get/{app}/{env}", UriKind.Relative));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadAsStringAsync();
        }
        catch
        {
            return null;
        }
    }
}