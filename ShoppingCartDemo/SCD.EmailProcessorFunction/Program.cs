using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Messaging.ServiceBus;
using SCD.EmailProcessorFunction.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddSingleton(busService =>
        {
            return new ServiceBusClient(Environment.GetEnvironmentVariable("ServiceBusConnectionName"));
        });

        services.AddHttpClient("EmailTriggerLogicApp", u => u.BaseAddress =
            new Uri(Environment.GetEnvironmentVariable("EmailTriggerLogicApp")));

        services.AddScoped<EmailService>();
    }).Build();

host.Run();
