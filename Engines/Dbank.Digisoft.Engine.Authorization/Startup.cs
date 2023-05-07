using Dbank.Digisoft.Engine.Authorization.Models;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace Dbank.Digisoft.Engine.Authorization
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
            IdentityModelEventSource.ShowPII = true;

            var clients = _configuration.GetSection("Clients").Get<List<Client>>();
            var identityResources = _configuration.GetSection("IdentityResources").Get<List<IdentityResource>>();
            var apiResources = _configuration.GetSection("ApiResources").Get<List<ApiResource>>();
            var apiScopes = _configuration.GetSection("ApiScopes").Get<List<ApiScope>>();
            var testUsers = _configuration.GetSection("TestUsers").Get<List<TestUser>>();
            services.AddIdentityServer()
                    .AddInMemoryClients(clients)
                    .AddInMemoryIdentityResources(identityResources)
                    .AddInMemoryApiResources(apiResources)
                    .AddInMemoryApiScopes(apiScopes)
                    .AddTestUsers(testUsers)
                    .AddDeveloperSigningCredential();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
