using Demo.Hotel.Cancellations.Shared;

namespace Demo.Hotel.Cancellations.Infrastructure.DataAccess;

public interface ICommand
{
    
}

public interface ICommandHandler<TCommand> where TCommand:ICommand
{
    Task<Result> ExecuteAsync(TCommand command);
}