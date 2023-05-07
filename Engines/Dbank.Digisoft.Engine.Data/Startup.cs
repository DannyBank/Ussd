using Dbank.Digisoft.Engine.Data.Extensions;
using Dbank.Digisoft.Ussd.Data.Extensions;
using Dbank.Digisoft.Ussd.SDK.Extensions;

namespace Dbank.Digisoft.Engine.Data {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDataServices()
                .AddSessionServices()
                .AddBusinessDI();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }
    }

}
