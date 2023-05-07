using Dbank.Digisoft.Api.Data;
using Dbank.Digisoft.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace Dbank.Digisoft.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("OAuth2")
                    .AddIdentityServerAuthentication("OAuth2", options => 
                    {
                        options.ApiName = "churchapi";
                        options.Authority = "http://localhost:55334";
                        options.RequireHttpsMetadata = false;
                    });
            services.Configure<AppSettings>(_configuration.GetSection("App"));
            services.Configure<Outputs>(_configuration.GetSection("Outputs"));
            services.AddDataServices()
                    .AddBusinessLogic()
                    .AddMvc();
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IdentityModelEventSource.ShowPII = true;
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
            });
        }
    }
}
