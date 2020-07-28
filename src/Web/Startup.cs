using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Extensions;

namespace Web
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
            services.AddOptions();
            services.AddSwagger();
            services.AddApplicationServices(Configuration);
           // services.AddDatabases(Configuration);
           // services.AddQueueSystem(Configuration);
            services.AddMediatR(typeof(Startup));
            services.AddControllers();
            services.AddApiConfiguration();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseHttpsRedirection();
            app.UseRemoveUnsafeServerHeaders();
            app.UseErrorHandler();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
            }

            //app.UseHangfireDashboard();
            app.UseCustomStatusCode();
            app.UseRouting();
            app.UseAuthorization();
            app.UseLoggerEnricher();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
