using System.Diagnostics;
using AspNetCoreAnatomySamples.Core;
using AspNetCoreAnatomySamples.Customisation;
using AspNetCoreAnatomySamples.Customisation.ExceptionFilter;
using AspNetCoreAnatomySamples.Customisation.ModelBinding;
using AspNetCoreAnatomySamples.Customisation.ResultFilter;
using AspNetCoreAnatomySamples.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreAnatomySamples
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateRangeBinderProvider());
                options.Filters.Add<HandleExceptionFilter>();
                options.Filters.Add<LastModifiedResultFilter>();
            });

            services.AddMemoryCache();
            services.AddSingleton<IBookRepository, BookRepository>();
            services.AddSingleton<IMetricRecorder, MetricRecorder>();
            services.AddResponseCompression(opt => opt.EnableForHttps = true); // Make sure you know what you're doing with this setting, See BREACH vulnerability.

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            // Inline middleware example
            app.Use(async (ctx, next) =>
            {
                var stopWatch = Stopwatch.StartNew();

                await next(); // call next middleware in pipeline

                stopWatch.Stop();

                var recorder = ctx.RequestServices.GetRequiredService<IMetricRecorder>();

                recorder.RecordRequest(ctx.Response.StatusCode, stopWatch.ElapsedMilliseconds);
            });

            // Direct registration
            app.UseMiddleware<MetricMiddleware>();

            // Extension method registration: best practice for libraries!
            app.UseMetrics();

            // Adds endpoint logging before "UseRouting", so this is not "Endpoint aware"
            app.UseMiddleware<EndpointLoggingMiddleware>(); 

            app.UseRouting(); // This maps the request to a suitable Endpoint (if possible)

            // Adds endpoint logging again, now "Endpoint aware"
            app.UseMiddleware<EndpointLoggingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            // Dispatches requests to endpoints. The Action here is where we map routes to the route builder.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
