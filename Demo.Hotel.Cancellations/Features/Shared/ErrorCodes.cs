namespace Demo.Hotel.Cancellations.Features.Shared;

public static class ErrorCodes
{
    public const string EmptyData = nameof(EmptyData);
    public const string InvalidRequest = nameof(InvalidRequest);
    public const string InternalError = nameof(InternalError);
    public const string PublishMessageError = nameof(PublishMessageError);
    public const string MessageReadError = nameof(MessageReadError);
}

public static class ErrorMessages
{
    public const string EmptyData = "empty data";
    public const string InvalidRequest = "invalid request";
    public const string InternalError = "internal error occurred";
    public const string PublishMessageError = "error occurred when publishing the message";
    public const string QueueDoesNotExist = "queue does not exist";
    public const string MessageReadError = "error occurred when reading message";
}