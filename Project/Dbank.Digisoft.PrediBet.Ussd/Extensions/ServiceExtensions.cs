using Dbank.Digisoft.PrediBet.Ussd.Helpers;
using Dbank.Digisoft.Ussd.Data.Abstractions;
using Dbank.Digisoft.Ussd.Data.Clients;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Dbank.Digisoft.PrediBet.Ussd.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBusinessDI(this IServiceCollection services)
        {
            services.AddTransient<UssdHandler>();
            services.AddTransient<GatesHelper>();
            services.AddTransient<MenuStore>();
            services.AddTransient<NavigationStack>();
            services.AddTransient<UssdExceptionHandler>();
            services.AddTransient<MenuHandler>();
            services.AddTransient<IPrediBetClient, PrediBetClient>();
            services.Scan(scan =>
                scan.FromEntryAssembly()
                    .AddClasses(c => c.AssignableTo<IMenuHandler>())
                    .AsSelfWithInterfaces());
            return services;
        }
    }
}