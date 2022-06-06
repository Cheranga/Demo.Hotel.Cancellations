namespace Demo.Hotel.Cancellations.Shared;

public class ErrorResponse
{
    public ErrorResponse()
    {
        Errors = new List<ErrorData>();
    }

    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }

    public List<ErrorData> Errors { get; set; }
}

public class ErrorData
{
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}