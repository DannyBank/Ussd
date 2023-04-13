using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Helper;

namespace Dbank.Digisoft.Engine.Data.Extensions {
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBusinessDI(this IServiceCollection services)
        {
            services.AddTransient<PrediBetDataHelper>();
            services.AddTransient<ChurchDataHelper>();
            return services;
        }
    }
}