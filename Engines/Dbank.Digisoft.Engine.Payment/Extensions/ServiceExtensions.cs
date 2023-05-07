using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.SDK.Helper;

namespace Dbank.Digisoft.Engine.Data.Payment {
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBusinessDI(this IServiceCollection services)
        {
            services.AddTransient<DbHelper>();
            services.AddTransient<PaymentDataHelper>();
            services.AddTransient<HubtelHelper>();
            services.AddTransient<PaystackHelper>();
            return services;
        }
    }
}