using Dbank.Digisoft.PrediBet.Ussd.Helpers;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Handlers;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Helper;

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
            services.AddTransient<IApplicationDataHelper, ApplicationDataHelper>();
            services.Scan(scan =>
                scan.FromEntryAssembly()
                    .AddClasses(c => c.AssignableTo<IMenuHandler>())
                    .AsSelfWithInterfaces());
            return services;
        }
    }
}