using System.ComponentModel.DataAnnotations;
using Demo.Hotel.Cancellations.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Hotel.Cancellations.Features.AcceptCancellation;

[ApiController]
public class AcceptCancellationController : ControllerBase
{
    private readonly IResponseGenerator<AcceptCancellationRequest> _responseGenerator;
    private readonly IAcceptCancellationService _service;

    public AcceptCancellationController(IAcceptCancellationService service, IResponseGenerator<AcceptCancellationRequest> responseGenerator)
    {
        _service = service;
        _responseGenerator = responseGenerator;
    }

    [HttpPost("api/cancellation")]
    public async Task<IActionResult> AcceptCancellationAsync([FromBody] [Required] AcceptCancellationRequest request)
    {
        var operation = await _service.CancelAsync(request);
        return _responseGenerator.GetResponse(operation);
    }
}