using Dbank.Digisoft.Api.Abstractions;
using Dbank.Digisoft.Api.Clients;
using Dbank.Digisoft.Api.Services;
using Dbank.Digisoft.Ussd.Data.Abstractions;
using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Helper;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Dbank.Digisoft.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services) {
            services.AddTransient<IDbHelper, DbHelper>();
            services.AddTransient<IApplicationDataHelper, ApplicationDataHelper>();
            return services;
        }

        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddTransient<HttpClient>();
            services.AddTransient<ISBClient, SBClient>();
            services.AddTransient<IHubtelService, HubtelService>();
            services.AddTransient<IPaymentClient, PaymentClient>();
            services.AddTransient<ISmsClient, SmsClient>();
            return services;
        }
    }
}
