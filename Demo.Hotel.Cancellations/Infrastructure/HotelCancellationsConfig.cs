namespace Demo.Hotel.Cancellations.Infrastructure;

public class HotelCancellationsConfig
{
    public string HotelCancellationsQueue { get; set; }
    public int PollingSeconds { get; set; }

    public int VisibilityInSeconds { get; set; }

    public HotelCancellationsConfig()
    {
        HotelCancellationsQueue = string.Empty;
    }
}