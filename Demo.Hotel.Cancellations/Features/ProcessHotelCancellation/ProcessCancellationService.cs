using Demo.Hotel.Cancellations.Extensions;
using Demo.Hotel.Cancellations.Features.CancelHotelBooking;
using Demo.Hotel.Cancellations.Features.SaveCancellations;
using Demo.Hotel.Cancellations.Infrastructure;
using Demo.Hotel.Cancellations.Infrastructure.DataAccess;
using Demo.Hotel.Cancellations.Infrastructure.Messaging;
using Demo.Hotel.Cancellations.Shared;
using Microsoft.FeatureManagement;

namespace Demo.Hotel.Cancellations.Features.ProcessHotelCancellation;

public class ProcessCancellationService : BackgroundService
{
    private readonly IFeatureManager _featureManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageReader _messageReader;
    private readonly ILogger<ProcessCancellationService> _logger;

    public ProcessCancellationService(IFeatureManager featureManager,
        IServiceProvider serviceProvider,
        IMessageReader messageReader,
        ILogger<ProcessCancellationService> logger)
    {
        _featureManager = featureManager;
        _serviceProvider = serviceProvider;
        _messageReader = messageReader;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var hotelCancellationConfig = scope.ServiceProvider.GetRequiredService<MessagingConfig>();
                var readMessageOperation = await _messageReader.ReadMessageAsync<CancelHotelBookingRequest>(new MessageReadOptions(hotelCancellationConfig.HotelCancellationsQueue, hotelCancellationConfig.VisibilityInSeconds), stoppingToken);

                if (!readMessageOperation.Status || readMessageOperation.Data == null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(hotelCancellationConfig.PollingSeconds), stoppingToken);
                    continue;
                }

                _logger.LogInformation("{CorrelationId} started processing cancellation request", readMessageOperation.Data.CorrelationId);

                var cancellationRequest = readMessageOperation.Data;
                var saveOperation = await SaveCancellationDataAsync(scope, cancellationRequest);

                await Task.Delay(TimeSpan.FromSeconds(hotelCancellationConfig.PollingSeconds), stoppingToken);
            }
        }
    }

    private async Task<Result> SaveCancellationDataAsync(IServiceScope scope, CancelHotelBookingRequest request)
    {
        var isEnable = await _featureManager.IsFeatureEnabled(Features.SaveLockTripCancellation);
        if (!isEnable)
        {
            _logger.LogWarning("{Feature} is not enabled", Features.SaveLockTripCancellation);
            return Result.Success();
        }
        
        var saveCancellationCommand = new SaveCancellationCommand
        {
            CorrelationId = request.CorrelationId,
            BookingReferenceId = request.BookingReferenceId,
            Provider = "AAA",
            Name = "Cheranga Hatangala",
            Email = "che123@gmail.com"
        };

        var commandHandler = scope.ServiceProvider.GetRequiredService<ICommandHandler<SaveCancellationCommand>>();

        return await commandHandler.ExecuteAsync(saveCancellationCommand);
    }
}