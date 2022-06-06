using Demo.Hotel.Cancellations.Infrastructure;
using Demo.Hotel.Cancellations.Shared;
using FluentValidation;

namespace Demo.Hotel.Cancellations.Features.AcceptCancellation;

public interface IAcceptCancellationService
{
    Task<Result> CancelAsync(AcceptCancellationRequest request);
}

public class AcceptCancellationService : IAcceptCancellationService
{
    private readonly IValidator<AcceptCancellationRequest> _validator;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<AcceptCancellationService> _logger;

    public AcceptCancellationService(IValidator<AcceptCancellationRequest> validator,
        IMessagePublisher messagePublisher,
        ILogger<AcceptCancellationService> logger)
    {
        _validator = validator;
        _messagePublisher = messagePublisher;
        _logger = logger;
    }
    
    public async Task<Result> CancelAsync(AcceptCancellationRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("{ErrorCode} invalid request", ErrorCodes.InvalidRequest);
            return Result.Failure(ErrorCodes.InvalidRequest, validationResult);
        }

        var publishOperation = await _messagePublisher.PublishAsync(request);
        return publishOperation;
    }
}