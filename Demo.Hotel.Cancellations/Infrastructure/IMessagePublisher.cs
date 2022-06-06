using Azure.Storage.Queues;
using Demo.Hotel.Cancellations.Shared;
using Newtonsoft.Json;

namespace Demo.Hotel.Cancellations.Infrastructure;

public interface IMessagePublisher
{
    Task<Result> PublishAsync<TData>(TData? data) where TData : IIdentifier;
}

public class MessagePublisher : IMessagePublisher
{
    private readonly HotelCancellationsConfig _config;
    private readonly QueueServiceClient _queueServiceClient;
    private readonly ILogger<MessagePublisher> _logger;

    public MessagePublisher(HotelCancellationsConfig config, QueueServiceClient queueServiceClient, ILogger<MessagePublisher> logger)
    {
        _config = config;
        _queueServiceClient = queueServiceClient;
        _logger = logger;
    }
    
    public async Task<Result> PublishAsync<TData>(TData? data) where TData : IIdentifier
    {
        var queueClient = _queueServiceClient.GetQueueClient(_config.HotelCancellationsQueue);
        
        if (data == null)
        {
            return Result.Failure(ErrorCodes.EmptyData, ErrorMessages.EmptyData);
        }

        var content = JsonConvert.SerializeObject(data, Formatting.None);

        var sendMessageOperation = await queueClient.SendMessageAsync(content);

        if (sendMessageOperation.GetRawResponse().IsError)
        {
            _logger.LogError("{CorrelationId} error occurred when publishing the message", data.CorrelationId);
            return Result.Failure(ErrorCodes.PublishMessageError, ErrorMessages.PublishMessageError);
        }

        return Result.Success();
    }
}