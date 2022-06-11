using AutoFixture;
using Demo.Hotel.Cancellations.Features.CancelHotelBooking;
using Demo.Hotel.Cancellations.Infrastructure.Messaging;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using TestStack.BDDfy;
using Result = Demo.Hotel.Cancellations.Shared.Result;

namespace Demo.Hotel.Cancellations.Tests;

public class CancelBookingServiceTests
{
    private readonly Fixture _fixture;
    private CancelHotelBookingRequest _request;
    private Mock<IValidator<CancelHotelBookingRequest>> _validator;
    private  Mock<IMessagePublisher> _messagePublisher;
    private Mock<ILogger<CancelHotelBookingService>> _logger;
    private CancelHotelBookingService _service;
    private Result _operation;


    public CancelBookingServiceTests()
    {
        _fixture = new Fixture();
        _request = _fixture.Create<CancelHotelBookingRequest>();
        _operation = Result.Success();
        
        _validator = new Mock<IValidator<CancelHotelBookingRequest>>();
        _messagePublisher = new Mock<IMessagePublisher>();
        _logger = new Mock<ILogger<CancelHotelBookingService>>();

        _service = new CancelHotelBookingService(_validator.Object, _messagePublisher.Object, _logger.Object);
    }
    
    [Fact]
    public void InvalidRequestMustFail()
    {
        this.Given(x => x.GivenInvalidRequest())
            .When(x => WhenRequestIsProcessed())
            .Then(x => ThenMustReturnFailure())
            .BDDfy();

    }

    private void ThenMustReturnFailure()
    {
        _operation.Should().NotBeNull();
        _operation.Status.Should().BeFalse();
        _operation.ValidationResult.IsValid.Should().BeFalse();
        _operation.ValidationResult.Errors.Should().ContainSingle(failure => failure.PropertyName == "errorcode" && failure.ErrorMessage == "error message");
    }

    private async Task WhenRequestIsProcessed()
    {
        _operation = await _service.CancelAsync(_request);
    }

    private void GivenInvalidRequest()
    {
        _validator.Setup(x => x.ValidateAsync(It.IsAny<CancelHotelBookingRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(new[]
        {
            new ValidationFailure("errorcode", "error message")
        }));
    }
}