using Dbank.Digisoft.PrediBet.Api.Abstractions;
using Dbank.Digisoft.PrediBet.Api.Clients;
using Dbank.Digisoft.PrediBet.Api.Services;
using Dbank.Digisoft.PrediBet.Ussd.Data.Abstractions;
using Dbank.Digisoft.PrediBet.Ussd.Data.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Helper;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Dbank.Digisoft.PrediBet.Api.Extensions
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
