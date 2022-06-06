using Demo.Hotel.Cancellations.Features.AcceptCancellation;
using Demo.Hotel.Cancellations.Infrastructure;
using Demo.Hotel.Cancellations.Shared;
using FluentValidation;

namespace Demo.Hotel.Cancellations;

public static class Bootstrapper
{
    public static void RegisterDependencies(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        RegisterHandlers(services);
        RegisterResponseGenerators(services);
        RegisterValidators(services);
        RegisterInfrastructure(services);
    }

    private static void RegisterHandlers(IServiceCollection services)
    {
        services.AddScoped<IAcceptCancellationService, AcceptCancellationService>();
    }

    private static void RegisterResponseGenerators(IServiceCollection services)
    {
        services.AddScoped<IResponseGenerator<AcceptCancellationRequest>, AcceptCancellationResponseGenerator>();
    }

    private static void RegisterInfrastructure(IServiceCollection services)
    {
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
    }

    private static void RegisterValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ModelValidatorBase<>).Assembly);
    }
}