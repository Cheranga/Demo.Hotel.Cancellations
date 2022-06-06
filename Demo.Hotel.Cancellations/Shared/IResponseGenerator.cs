using Microsoft.AspNetCore.Mvc;

namespace Demo.Hotel.Cancellations.Shared;

public interface IResponseGenerator<TRequest>
{
    IActionResult GetResponse(Result operation);
}

