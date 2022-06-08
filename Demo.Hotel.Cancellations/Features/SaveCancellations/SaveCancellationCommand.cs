using Demo.Hotel.Cancellations.Infrastructure.DataAccess;

namespace Demo.Hotel.Cancellations.Features.SaveCancellations;

public class SaveCancellationCommand : ICommand
{
    public SaveCancellationCommand()
    {
        Provider = string.Empty;
        CorrelationId = string.Empty;
        BookingReferenceId = string.Empty;
        Name = string.Empty;
        Email = string.Empty;
    }

    public string Provider { get; set; }

    public string CorrelationId { get; set; }

    public string BookingReferenceId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    
}