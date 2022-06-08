using Demo.Hotel.Cancellations.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Hotel.Cancellations.Features.Shared;

public interface IResponseGenerator<TRequest>
{
    IActionResult GetResponse(TRequest request, Result operation);
}

