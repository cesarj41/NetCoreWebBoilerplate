using Destructurama;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Web.Exceptions;
using Web.Middlewares;
using Web.Models;

namespace Web.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void UseSwagger(this IApplicationBuilder app)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }

        public static void UseRemoveUnsafeServerHeaders(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Strict-Transport-Security","max-age=<seconds> ; includeSubDomains");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("Expect-CT","max-age=43200, enforce");
                context.Response.Headers.Add("X-XSS-Protection","0");
                await next();
            });
        }

        public static IHostBuilder UseSerilog(this IHostBuilder builder)
        {
            return builder
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Destructure.UsingAttributes());
        }

        public static IHostBuilder UseApplicationConfiguration(this IHostBuilder builder)
        {
            return builder
                .ConfigureAppConfiguration((context, builder) => 
                {
                    builder
                        .AddConfiguration(context.Configuration)
                        .Build();
                });
        }

        public static void UseCustomStatusCode(this IApplicationBuilder app)
        {
             app.UseStatusCodePages( async ctx => 
             {
                string requestPath = ctx.HttpContext.Request.Path.Value;
                string httpMethod = ctx.HttpContext.Request.Method;
                string problem = "Invalid request";
                int statusCode = ctx.HttpContext.Response.StatusCode;

                if (statusCode == 401) problem = "Unauthorized request";
                else if (statusCode == 403) problem = "Forbidden request";
                else if (statusCode == 404) problem = "Resource not found";
                else if (statusCode == 405) problem = "Invalid request, method not allowed";
                else if (statusCode == 406) problem = "Invalid request, not acceptable";

                var errorDetails = new ErrorDetails(problem);

                Log.Warning(
                    "Request for path: {path}, method: {httpMethod} was invalid, returned status code: {status}, result: {@result}",
                    requestPath,
                    httpMethod,
                    statusCode,
                    errorDetails
                );
                await ctx.HttpContext.Response.SendAsync(errorDetails);
            });
        }

        public static void UseLoggerEnricher(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoggerEnricherMiddleware>();
        }

        public static void UseErrorHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp => errorApp.Run(async context => 
            {
                int statusCode = 500;
                object actionName = "";
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var errorDetails = new ErrorDetails("Internal server error");
                var exception = errorFeature.Error;

                context.Items.TryGetValue("actionName", out actionName);

                if (exception is BaseException baseException)
                {
                    statusCode = baseException.HttpStatusCode;
                    errorDetails = new ErrorDetails(baseException.Errors);
                    Log.Warning(
                        exception,
                        "An Exception occured ! instance: {instance}",
                        errorDetails.instance
                    );

                    Log.Warning(
                        "Request for path: {path}, action: {action} was invalid, returned status code: {status}, result: {@result}",
                        context.Request.Path,
                        actionName as string,
                        statusCode,
                        errorDetails
                    );
                }
                else
                {
                    Log.Error(
                        exception,
                        "An unexpected error occurred! instance: {instance}",
                        errorDetails.instance
                    );
                    Log.Warning(
                        "An error ocurred proccessing request for path: {path}, action: {action}, returned status code: {status}, result: {@result}",
                        context.Request.Path,
                        actionName as string,
                        statusCode,
                        errorDetails
                    );
                }

                await context.Response.Status(statusCode).SendAsync(errorDetails);
            }));
        }

    }
}