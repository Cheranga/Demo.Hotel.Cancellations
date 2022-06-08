using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage;
using Azure.Storage.Queues;
using Demo.Hotel.Cancellations.Features;
using Demo.Hotel.Cancellations.Features.CancelHotelBooking;
using Demo.Hotel.Cancellations.Features.ProcessHotelCancellation;
using Demo.Hotel.Cancellations.Features.SaveCancellations;
using Demo.Hotel.Cancellations.Features.Shared;
using Demo.Hotel.Cancellations.Infrastructure;
using Demo.Hotel.Cancellations.Infrastructure.DataAccess;
using Demo.Hotel.Cancellations.Infrastructure.Messaging;
using Demo.Hotel.Cancellations.Shared;
using FluentValidation;
using Microsoft.Extensions.Azure;
using Serilog;

namespace Demo.Hotel.Cancellations;

public static class Bootstrapper
{
    public static void RegisterDependencies(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        RegisterLogging(builder);
        RegisterConfigurations(builder);
        RegisterHandlers(services);
        RegisterResponseGenerators(services);
        RegisterValidators(services);
        RegisterInfrastructure(builder);
    }

    private static void RegisterLogging(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_, configuration) =>
        {
            configuration.MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
        });
    }

    private static void RegisterHandlers(IServiceCollection services)
    {
        services.AddScoped<ICancelHotelBookingService, CancelHotelBookingService>();
        services.AddHostedService<ProcessCancellationService>();
    }

    private static void RegisterResponseGenerators(IServiceCollection services)
    {
        services.AddScoped<IResponseGenerator<CancelHotelBookingRequest>, CancelHotelBookingResponseGenerator>();
    }

    private static void RegisterInfrastructure(WebApplicationBuilder builder)
    {
        RegisterAzureClients(builder);
        builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();
        builder.Services.AddSingleton<IMessageReader, MessageReader>();

        builder.Services.AddScoped<ICommandHandler<SaveCancellationCommand>, SaveCancellationCommandHandler>();
    }

    private static void RegisterValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ModelValidatorBase<>).Assembly);
    }

    private static void RegisterAzureClients(WebApplicationBuilder builder)
    {
        var storageAccount = builder.Configuration["StorageAccount"];

        builder.Services.AddAzureClients(x =>
        {
            if (builder.Environment.IsDevelopment())
            {
                x.AddQueueServiceClient(storageAccount);
                x.AddTableServiceClient(storageAccount);
                return;
            }

            var credentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                ExcludeManagedIdentityCredential = false
            });
            x.AddQueueServiceClient(new Uri($"https://{storageAccount}.queue.core.windows.net")).WithCredential(credentials);
            x.AddTableServiceClient(new Uri($"https://{storageAccount}.table.core.windows.net")).WithCredential(credentials);
        });
    }

    private static void RegisterConfigurations(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(_ =>
        {
            var configuration = builder.Configuration;

            // if (builder.Environment.IsDevelopment())
            // {
            var messagingConfig = configuration.GetSection(nameof(MessagingConfig)).Get<MessagingConfig>();
            return messagingConfig;
            // }
            //
            // var hotelCancellationQueue = configuration[nameof(MessagingConfig.HotelCancellationsQueue)];
            // var cancellationsTable = configuration[nameof(MessagingConfig.CancellationsTable)];
            // int.TryParse(configuration[nameof(MessagingConfig.PollingSeconds)], out var pollingSeconds);
            // int.TryParse(configuration[nameof(MessagingConfig.VisibilityInSeconds)], out var visibilityInSeconds);
            //
            // return new MessagingConfig
            // {
            //     CancellationsTable = cancellationsTable,
            //     HotelCancellationsQueue = hotelCancellationQueue,
            //     PollingSeconds = pollingSeconds,
            //     VisibilityInSeconds = visibilityInSeconds
            // };
        });
    }
}