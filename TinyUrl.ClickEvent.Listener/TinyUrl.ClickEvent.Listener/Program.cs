using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TinyUrl.ClickEvent.Listener.Context;
using TinyUrl.ClickEvent.Listener.Interfaces;
using TinyUrl.ClickEvent.Listener.Models;
using TinyUrl.ClickEvent.Listener.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IUrlMappingService, UrlMappingService>();

        services.Configure<MongoDbOptions>(configuration.GetSection("MongoDbOptions"));

        services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>().Value;
            return new MongoClient(options.ConnectionString);
        });

        services.AddScoped<MongoDbContext>();
    })
    .Build();

host.Run();
