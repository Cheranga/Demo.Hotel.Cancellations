using Demo.Hotel.Cancellations.Features.Shared;
using Demo.Hotel.Cancellations.Infrastructure.Messaging;
using Demo.Hotel.Cancellations.Shared;
using FluentValidation;
using Microsoft.FeatureManagement;

namespace Demo.Hotel.Cancellations.Features.CancelHotelBooking;

public interface ICancelHotelBookingService
{
    Task<Result> CancelAsync(CancelHotelBookingRequest request);
}

public class CancelHotelBookingService : ICancelHotelBookingService
{
    private readonly IValidator<CancelHotelBookingRequest> _validator;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<CancelHotelBookingService> _logger;

    public CancelHotelBookingService(IValidator<CancelHotelBookingRequest> validator,
        IMessagePublisher messagePublisher,
        ILogger<CancelHotelBookingService> logger)
    {
        _validator = validator;
        _messagePublisher = messagePublisher;
        _logger = logger;
    }
    
    public async Task<Result> CancelAsync(CancelHotelBookingRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("{ErrorCode} invalid request received {@Request}", ErrorCodes.InvalidRequest, request);
            return Result.Failure(ErrorCodes.InvalidRequest, validationResult);
        }
        
        var publishOperation = await _messagePublisher.PublishAsync(request);
        
        _logger.LogInformation("{CorrelationId} cancel booking request submitted", request.CorrelationId);
        return publishOperation;
    }
}