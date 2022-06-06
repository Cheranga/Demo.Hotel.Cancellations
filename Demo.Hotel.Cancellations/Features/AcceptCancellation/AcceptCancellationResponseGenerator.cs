using System.Net;
using Demo.Hotel.Cancellations.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Hotel.Cancellations.Features.AcceptCancellation;

public class AcceptCancellationResponseGenerator : IResponseGenerator<AcceptCancellationRequest>
{
    public IActionResult GetResponse(Result operation)
    {
        if (operation.Status)
        {
            return new AcceptedResult();
        }

        return GetErrorMessage(operation);
    }

    private IActionResult GetErrorMessage(Result operation)
    {
        ErrorResponse errorResponse;
        switch (operation.ErrorCode)
        {
            case ErrorCodes.InvalidRequest:
                errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.InvalidRequest,
                    ErrorMessage = ErrorMessages.InvalidRequest,
                    Errors = operation.ValidationResult.Errors.Select(x => new ErrorData
                    {
                        ErrorCode = x.PropertyName,
                        ErrorMessage = x.ErrorMessage
                    }).ToList()
                };

                return new BadRequestObjectResult(errorResponse);
            
            default:
                errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.InternalError,
                    ErrorMessage = ErrorMessages.InternalError,
                    Errors = operation.ValidationResult.Errors.Select(x => new ErrorData
                    {
                        ErrorCode = x.ErrorCode,
                        ErrorMessage = x.ErrorMessage
                    }).ToList()
                };

                return new ObjectResult(errorResponse)
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError
                };
        }
    }
}