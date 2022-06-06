namespace Demo.Hotel.Cancellations.Shared;

public class ErrorCodes
{
    public const string EmptyData = nameof(EmptyData);
    public const string InvalidRequest = nameof(InvalidRequest);
    public const string InternalError = nameof(InternalError);
}

public class ErrorMessages
{
    public const string EmptyData = "empty data";
    public const string InvalidRequest = "invalid request";
    public const string InternalError = "internal error occurred";
}