using Demo.Hotel.Cancellations.Features.CancelHotelBooking;
using Demo.Hotel.Cancellations.Infrastructure;
using Demo.Hotel.Cancellations.Infrastructure.Messaging;

namespace Demo.Hotel.Cancellations.Features.ProcessHotelCancellation;

public class ProcessCancellationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageReader _messageReader;
    private readonly ILogger<ProcessCancellationService> _logger;

    public ProcessCancellationService(IServiceProvider serviceProvider,
        IMessageReader messageReader,
        ILogger<ProcessCancellationService> logger)
    {
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

                var cancellationRequest = readMessageOperation.Data;
                _logger.LogInformation("{CorrelationId} processing cancellation request", cancellationRequest.CorrelationId);
                // TODO: The rest of the process
                await Task.Delay(TimeSpan.FromSeconds(hotelCancellationConfig.PollingSeconds), stoppingToken);
            }
        }
    }
}