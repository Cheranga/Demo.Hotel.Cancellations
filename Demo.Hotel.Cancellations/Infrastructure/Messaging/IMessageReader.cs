using Azure.Storage.Queues;
using Demo.Hotel.Cancellations.Shared;
using Newtonsoft.Json;

namespace Demo.Hotel.Cancellations.Infrastructure.Messaging;

public interface IMessageReader
{
    Task<Result<TData>> ReadMessageAsync<TData>(MessageReadOptions options, CancellationToken cancellationToken) where TData : class;
}

public class MessageReadOptions
{
    public string QueueName { get; }
    public int VisibilityInSeconds { get; }

    public MessageReadOptions(string queueName, int visibilityInSeconds = 60)
    {
        QueueName = queueName;
        VisibilityInSeconds = visibilityInSeconds;
    }
}

public class MessageReader : IMessageReader
{
    private readonly QueueServiceClient _queueServiceClient;
    private readonly ILogger<MessageReader> _logger;

    private readonly JsonSerializerSettings _serializerSettings;

    public MessageReader(QueueServiceClient queueServiceClient, ILogger<MessageReader> logger)
    {
        _queueServiceClient = queueServiceClient;
        _logger = logger;

        _serializerSettings = new JsonSerializerSettings
        {
            Error = (_, args) => args.ErrorContext.Handled = true
        };
    }

    public async Task<Result<TData>> ReadMessageAsync<TData>(MessageReadOptions options, CancellationToken cancellationToken) where TData : class
    {
        var queueClient = _queueServiceClient.GetQueueClient(options.QueueName);
        var exists = await queueClient.ExistsAsync(cancellationToken);
        if (!exists)
        {
            throw new Exception(ErrorMessages.QueueDoesNotExist);
        }

        try
        {
            var queueReadOperation = await queueClient.ReceiveMessageAsync(TimeSpan.FromSeconds(options.VisibilityInSeconds), cancellationToken);

            var queueMessage = queueReadOperation.Value;
            if (queueMessage == null)
            {
                return Result<TData>.Success(default!);
            }

            var messageData = JsonConvert.DeserializeObject<TData>(queueMessage.MessageText, _serializerSettings);
            await queueClient.DeleteMessageAsync(queueMessage.MessageId, queueMessage.PopReceipt, cancellationToken);
            
            return Result<TData>.Success(messageData!);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, ErrorMessages.MessageReadError);
            return Result<TData>.Failure(ErrorCodes.MessageReadError, ErrorMessages.MessageReadError);
        }
    }
}