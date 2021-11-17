using AspNetCoreAnatomySamples.Core;
using AspNetCoreAnatomySamples.Customisation;
using AspNetCoreAnatomySamples.Customisation.ExceptionFilter;
using AspNetCoreAnatomySamples.Customisation.ModelBinding;
using AspNetCoreAnatomySamples.Customisation.ResultFilter;
using AspNetCoreAnatomySamples.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Configure application services.

builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new DateRangeBinderProvider());
    options.Filters.Add<HandleExceptionFilter>();
    options.Filters.Add<LastModifiedResultFilter>();
});

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddSingleton<IMetricRecorder, MetricRecorder>();
builder.Services.AddResponseCompression(opt => opt.EnableForHttps = true); // Make sure you know what you're doing with this setting, See BREACH vulnerability.

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
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

app.MapControllers();

app.Run();
