namespace Demo.Hotel.Cancellations.Features.Shared;

public static class ErrorCodes
{
    public const string EmptyData = nameof(EmptyData);
    public const string InvalidRequest = nameof(InvalidRequest);
    public const string InternalError = nameof(InternalError);
    public const string PublishMessageError = nameof(PublishMessageError);
    public const string MessageReadError = nameof(MessageReadError);
    public const string SaveCancelDataError = nameof(SaveCancelDataError);
    public const string InvalidSaveCommand = nameof(InvalidSaveCommand);
}

public static class ErrorMessages
{
    public const string EmptyData = "empty data";
    public const string InvalidRequest = "invalid request";
    public const string InternalError = "internal error occurred";
    public const string PublishMessageError = "error occurred when publishing the message";
    public const string QueueDoesNotExist = "queue does not exist";
    public const string MessageReadError = "error occurred when reading message";
    public const string SaveCancelDataError = "error occurred when saving cancellation data";
    public const string InvalidSaveCommand = "invalid save cancel command";
}