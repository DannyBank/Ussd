using Dbank.Digisoft.TranscriptDelivery.Ussd.Extensions;
using Dbank.Digisoft.Ussd.SDK.Extensions;

namespace Dbank.Digisoft.TranscriptDelivery.Ussd {
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(_configuration.GetSection("App"));
            services.Configure<AppStrings>(_configuration.GetSection("Outputs"));
            services.Configure<MenuData>(_configuration.GetSection("MenuData"));
            services.AddSessionServices()
                    .AddBusinessDI();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
