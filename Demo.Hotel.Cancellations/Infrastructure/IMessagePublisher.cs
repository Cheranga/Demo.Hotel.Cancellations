using Demo.Hotel.Cancellations.Shared;
using Newtonsoft.Json;

namespace Demo.Hotel.Cancellations.Infrastructure;

public interface IMessagePublisher
{
    Task<Result> PublishAsync<TData>(TData? data) where TData : class;
}

public class MessagePublisher : IMessagePublisher
{
    public Task<Result> PublishAsync<TData>(TData? data) where TData : class
    {
        if (data == null)
        {
            return Task.FromResult(Result.Failure(ErrorCodes.EmptyData, ErrorMessages.EmptyData));
        }

        var content = JsonConvert.SerializeObject(data, Formatting.None);

        // TODO: publish the message to the message broker
        return Task.FromResult(Result.Success());
    }
}