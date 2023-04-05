using Microsoft.Extensions.DependencyInjection;

namespace Dbank.Digisoft.Engine.Session.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            return services;
        }
    }
}