using Demo.Hotel.Cancellations.Shared;

namespace Demo.Hotel.Cancellations.Features.AcceptCancellation;

public class AcceptCancellationRequest : IDto
{
    public string BookingReferenceId { get; set; }
    public string CorrelationId { get; set; }

    public AcceptCancellationRequest()
    {
        BookingReferenceId = string.Empty;
        CorrelationId = string.Empty;
    }
}