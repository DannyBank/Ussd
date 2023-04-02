using System.Text.Json.Serialization;
using Dbank.Digisoft.PrediBet.Ussd.Data.Extensions;
using Dbank.Digisoft.PrediBet.Engine.Session.Scheduler;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dbank.Digisoft.PrediBet.Engine.Session
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataServices()
                .AddSessionServices();
            services.AddSingleton<IHostedService, SessionCleaningTask>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
        }
    }
    
}
