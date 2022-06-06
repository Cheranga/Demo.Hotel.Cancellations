namespace Demo.Hotel.Cancellations.Shared;

public interface IIdentifier
{
    string CorrelationId { get; set; }
}