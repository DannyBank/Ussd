using Dbank.Digisoft.Church.Ussd.Abstractions;
using Dbank.Digisoft.Church.Ussd.Common;
using Dbank.Digisoft.Church.Ussd.Helpers;
using Dbank.Digisoft.Ussd.Data.Abstractions;
using Dbank.Digisoft.Ussd.Data.Clients;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Handlers;
using Dbank.Digisoft.Ussd.SDK.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace Dbank.Digisoft.Church.Ussd.Extensions {
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
            services.AddTransient<IViewHelper, ViewHelper>();
            services.AddTransient<ChurchDataHelper>();
            services.AddTransient<IChurchClient, ChurchClient>();
            services.Scan(scan =>
                scan.FromEntryAssembly()
                    .AddClasses(c => c.AssignableTo<IMenuHandler>())
                    .AsSelfWithInterfaces());
            return services;
        }
    }
}