using Azure.Data.Tables;
using Demo.Hotel.Cancellations.Features.Shared;
using Demo.Hotel.Cancellations.Shared;
using FluentValidation;

namespace Demo.Hotel.Cancellations.Infrastructure.DataAccess;

public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly IValidator<TCommand> _validator;
    private readonly TableServiceClient _serviceClient;
    private readonly ILogger<CommandHandlerBase<TCommand>> _logger;

    protected CommandHandlerBase(IValidator<TCommand> validator, TableServiceClient serviceClient, ILogger<CommandHandlerBase<TCommand>> logger)
    {
        _validator = validator;
        _serviceClient = serviceClient;
        _logger = logger;
    }

    protected abstract string TableName { get; }
    protected abstract string ErrorCode { get; }
    protected abstract string ErrorMessage { get; }
    
    protected abstract TableUpdateMode UpsertMode { get; }

    public virtual async Task<Result> ExecuteAsync(TCommand command)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(ErrorMessages.InvalidSaveCommand);
            }
            
            var tableClient = _serviceClient.GetTableClient(TableName);
            await tableClient.CreateIfNotExistsAsync();

            var entity = GetTableEntity(command);
            var operation = await SaveAsync(tableClient, entity);

            return operation;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, ErrorMessage);
        }

        return Result.Failure(ErrorCode, ErrorMessage);
    }

    protected abstract TableEntity GetTableEntity(TCommand command);

    protected virtual async Task<Result> SaveAsync(TableClient client, TableEntity entity)
    {
        var response = await client.UpsertEntityAsync(entity, UpsertMode);
        if (response.IsError)
        {
            _logger.LogError("upsert error: {FailedReason}", response.ReasonPhrase);
            return Result.Failure(ErrorCode, ErrorMessage);
        }

        return Result.Success();
    }
}