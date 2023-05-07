using Dbank.Digisoft.TranscriptDelivery.Ussd.Helpers;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Handlers;
using Dbank.Digisoft.Ussd.SDK.Helper;

namespace Dbank.Digisoft.TranscriptDelivery.Ussd.Extensions {
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
            services.AddTransient<IApplicationDataHelper, ChurchDataHelper>();
            services.Scan(scan =>
                scan.FromEntryAssembly()
                    .AddClasses(c => c.AssignableTo<IMenuHandler>())
                    .AsSelfWithInterfaces());
            return services;
        }
    }
}