namespace CarAuctionApi.Patterns
{
    /// <summary>
    /// Defines a handler for a command without result
    /// </summary>
    /// <typeparam name="TCommand">The type of command being handled</typeparam>
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// Handles a command without result asynchronously
        /// </summary>
        Task HandleAsync(TCommand command);
    }

    /// <summary>
    /// Defines a handler for a command with result
    /// </summary>
    /// <typeparam name="TCommand">The type of command being handled</typeparam>
    /// <typeparam name="TResult">The type of result from the handler</typeparam>
    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// Handles a command with result asynchronously
        /// </summary>
        /// <returns>Result from the query</returns>
        Task<TResult> HandleAsync(TCommand command);
    }
}
