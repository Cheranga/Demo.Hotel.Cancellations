using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Queues;
using Demo.Hotel.Cancellations.Features.AcceptCancellation;
using Demo.Hotel.Cancellations.Infrastructure;
using Demo.Hotel.Cancellations.Shared;
using FluentValidation;
using Microsoft.Extensions.Azure;

namespace Demo.Hotel.Cancellations;

public static class Bootstrapper
{
    public static void RegisterDependencies(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        RegisterConfigurations(builder);
        RegisterHandlers(services);
        RegisterResponseGenerators(services);
        RegisterValidators(services);
        RegisterInfrastructure(builder);
    }

    private static void RegisterHandlers(IServiceCollection services)
    {
        services.AddScoped<IAcceptCancellationService, AcceptCancellationService>();
    }

    private static void RegisterResponseGenerators(IServiceCollection services)
    {
        services.AddScoped<IResponseGenerator<AcceptCancellationRequest>, AcceptCancellationResponseGenerator>();
    }

    private static void RegisterInfrastructure(WebApplicationBuilder builder)
    {
        RegisterAzureClients(builder);
        builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();
    }

    private static void RegisterValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ModelValidatorBase<>).Assembly);
    }

    private static void RegisterAzureClients(WebApplicationBuilder builder)
    {
        var storageConnectionString =  builder.Configuration["StorageConnectionString"];
        
        builder.Services.AddAzureClients(x => { x.AddQueueServiceClient(storageConnectionString); });
    }
    
    private static void RegisterConfigurations(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(_ =>
        {
            var databaseConfig = builder.Configuration.GetSection(nameof(HotelCancellationsConfig)).Get<HotelCancellationsConfig>();
            return databaseConfig;
        });
    }
}