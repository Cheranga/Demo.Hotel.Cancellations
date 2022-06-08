using Azure.Data.Tables;
using Demo.Hotel.Cancellations.Features.Shared;
using Demo.Hotel.Cancellations.Infrastructure.DataAccess;
using Demo.Hotel.Cancellations.Infrastructure.Messaging;
using FluentValidation;

namespace Demo.Hotel.Cancellations.Features.SaveCancellations;

public class SaveCancellationCommandHandler : CommandHandlerBase<SaveCancellationCommand>
{
    private readonly MessagingConfig _config;

    public SaveCancellationCommandHandler(IValidator<SaveCancellationCommand> validator,
        MessagingConfig config,
        TableServiceClient serviceClient,
        ILogger<SaveCancellationCommandHandler> logger) : base(validator, serviceClient, logger)
    {
        _config = config;
    }

    protected override string TableName => _config.CancellationsTable;
    protected override string ErrorCode => ErrorCodes.SaveCancelDataError;
    protected override string ErrorMessage => ErrorMessages.SaveCancelDataError;
    protected override TableUpdateMode UpsertMode => TableUpdateMode.Replace;

    protected override TableEntity GetTableEntity(SaveCancellationCommand command)
    {
        var entity = new TableEntity(command.Provider.ToUpper(), command.BookingReferenceId.ToUpper())
        {
            {nameof(SaveCancellationCommand.CorrelationId), command.CorrelationId},
            {nameof(SaveCancellationCommand.BookingReferenceId), command.BookingReferenceId},
            {nameof(SaveCancellationCommand.Name), command.Name},
            {nameof(SaveCancellationCommand.Email), command.Email}
        };

        return entity;
    }
}