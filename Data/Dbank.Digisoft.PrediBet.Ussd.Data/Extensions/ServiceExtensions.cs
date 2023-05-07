using Dbank.Digisoft.PrediBet.Ussd.Data.Abstractions;
using Dbank.Digisoft.PrediBet.Ussd.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dbank.Digisoft.PrediBet.Ussd.Data.Extensions {
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
            services.AddTransient<IDbHelper, DbHelper>();
            return services;
        }
    }
}
