using System.Runtime.Serialization;
using Demo.Hotel.Cancellations.Shared;
using Newtonsoft.Json;

namespace Demo.Hotel.Cancellations.Features.CancelHotelBooking;

public class CancelHotelBookingRequest : IIdentifier
{
    public string BookingReferenceId { get; set; }
    public string CorrelationId { get; set; }

    public CancelHotelBookingRequest()
    {
        CorrelationId = string.Empty;
        BookingReferenceId = string.Empty;
    }
}