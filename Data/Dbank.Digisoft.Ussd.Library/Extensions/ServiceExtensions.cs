using Dbank.Digisoft.Ussd.Data.Extensions;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Helper;
using Dbank.Digisoft.Ussd.SDK.Session.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dbank.Digisoft.Ussd.SDK.Extensions
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
