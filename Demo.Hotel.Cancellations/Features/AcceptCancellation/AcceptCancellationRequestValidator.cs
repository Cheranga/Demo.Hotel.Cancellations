using Demo.Hotel.Cancellations.Shared;
using FluentValidation;

namespace Demo.Hotel.Cancellations.Features.AcceptCancellation;

public class AcceptCancellationRequestValidator : ModelValidatorBase<AcceptCancellationRequest>
{
    public AcceptCancellationRequestValidator()
    {
        RuleFor(x => x.CorrelationId).NotNull().NotEmpty().WithMessage("correlationId is required");
        RuleFor(x => x.BookingReferenceId).NotNull().NotEmpty().WithMessage("booking reference id is required");
    }
}