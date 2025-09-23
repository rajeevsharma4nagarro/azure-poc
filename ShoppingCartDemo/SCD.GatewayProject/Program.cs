// using System.Runtime.InteropServices;
// using Ocelot.DependencyInjection;
// using Ocelot.Middleware;
// using Ocelot.Values;
//using SCD.GatewayProject.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;


var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
});

//builder.AddJwtAuthorization();
builder.Services.AddOcelot();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow_SCD_UI", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetValue<string>("AllowedOrigins").Split(","))
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("Allow_SCD_UI");
//app.UseAuthentication();
//app.UseAuthorization();
await app.UseOcelot();
app.Run();
