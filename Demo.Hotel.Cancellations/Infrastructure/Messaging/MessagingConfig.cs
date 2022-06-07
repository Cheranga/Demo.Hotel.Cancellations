namespace Demo.Hotel.Cancellations.Infrastructure.Messaging;

public class MessagingConfig
{
    public string CancellationsTable { get; set; }
    public string HotelCancellationsQueue { get; set; }
    public int PollingSeconds { get; set; }

    public int VisibilityInSeconds { get; set; }

    public MessagingConfig()
    {
        HotelCancellationsQueue = string.Empty;
        CancellationsTable = string.Empty;
    }
}