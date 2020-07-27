using System;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Data;
using Web.Models;

namespace Web.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerDocument();
        }

        
        public static void AddApiConfiguration(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = context =>
                    new BadRequestObjectResult(new ErrorDetails(
                        context.ModelState.Errors()
                    ));
            });
        }

        public static void AddDatabases(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContextPool<ApiContext>(options =>
            {
                options.UseSqlServer(config["DefaultConnection"]);
            });
        }

        public static void AddQueueSystem(this IServiceCollection services, IConfiguration config)
        {
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(config["DefaultConnection"], new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));
                
            services.AddHangfireServer();
        }

        public static void AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Register Application Services Here...
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
        } 
    }
}