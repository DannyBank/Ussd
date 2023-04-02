using Dbank.Digisoft.PrediBet.Ussd.Data.Extensions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Helper;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Abstractions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSessionServices(this IServiceCollection services)
        {
            services.AddDataServices();
            services.AddTransient<ISessionDataHelper, SessionDataHelper>();
            services.AddTransient<IApplicationDataHelper, ApplicationDataHelper>();
            services.AddTransient<SessionDataHelper>();
            services.AddHttpClient<ISessionServiceHelper, SessionServiceHelper>();
            return services;
        }
    }
}
