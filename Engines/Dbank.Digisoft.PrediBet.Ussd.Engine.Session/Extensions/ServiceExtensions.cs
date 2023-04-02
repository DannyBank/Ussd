using Microsoft.Extensions.DependencyInjection;

namespace Dbank.Digisoft.PrediBet.Engine.Session.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            return services;
        }
    }
}