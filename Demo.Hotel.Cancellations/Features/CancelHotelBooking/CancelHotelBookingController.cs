using System.ComponentModel.DataAnnotations;
using Demo.Hotel.Cancellations.Features.Shared;
using Demo.Hotel.Cancellations.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Hotel.Cancellations.Features.CancelHotelBooking;

[ApiController]
public class CancelHotelBookingController : ControllerBase
{
    private readonly IResponseGenerator<CancelHotelBookingRequest> _responseGenerator;
    private readonly ICancelHotelBookingService _service;

    public CancelHotelBookingController(ICancelHotelBookingService service, IResponseGenerator<CancelHotelBookingRequest> responseGenerator)
    {
        _service = service;
        _responseGenerator = responseGenerator;
    }

    [HttpPost("api/cancel/hotel")]
    public async Task<IActionResult> AcceptCancellationAsync([FromBody] [Required] CancelHotelBookingRequest request)
    {
        var operation = await _service.CancelAsync(request);
        return _responseGenerator.GetResponse(request, operation);
    }
}