using Dbank.Digisoft.Ussd.Data.Abstractions;
using Dbank.Digisoft.Ussd.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dbank.Digisoft.Ussd.Data.Extensions {
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
