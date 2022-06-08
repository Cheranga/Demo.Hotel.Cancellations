using System.IO.Hashing;
using Demo.Hotel.Cancellations.Features.Shared;
using FluentValidation;

namespace Demo.Hotel.Cancellations.Features.SaveCancellations;

public class SaveCancellationCommandValidator : ModelValidatorBase<SaveCancellationCommand>
{
    public SaveCancellationCommandValidator()
    {
        RuleFor(x => x.Provider).NotNull().NotEmpty().WithMessage("partition key providerId is required");
        RuleFor(x => x.BookingReferenceId).NotNull().NotEmpty().WithMessage("row key bookingReferenceId is required");
        RuleFor(x => x.CorrelationId).NotNull().NotEmpty().WithMessage("correlationId is required");
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("name is required");
        RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("email is required");
    }
}