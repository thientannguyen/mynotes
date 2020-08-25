namespace MyNotes.Application.SharedKernel
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using LanguageExt.Common;
    using MediatR;
    using Result = CommandHandlerResult;

    public abstract class CommandHandler<TCommand>
        : IRequestHandler<TCommand, Result<string>> where TCommand : Command
    {
        public Task<Result<string>> Handle(TCommand command, CancellationToken cancellationToken)
            => ExecuteCommand(command);

        protected abstract Task<Result<string>> ExecuteCommand(TCommand command);

        protected Result<string> Fail(string error) => Result.Fail(error);

        protected Result<string> Fail(Exception exception) => new Result<string>(exception);

        protected Result<string> Ok() => Result.Ok();

        protected Result<string> FailBecauseOfInvalidCommand(string message) =>
            new Result<string>(new InvalidCommandOrQueryException(message));
    }

    /// <summary>
    /// static factory methods to return command handler result
    /// </summary>
    public static class CommandHandlerResult
    {
        /// <summary>
        ///  indicates that no issue occurs. Empty string is used,
        ///  because when map to a controller result an OK (200) result is returned
        /// </summary>
        private static readonly string OkValue = string.Empty;
        private static readonly Func<string, Exception> Exception = error => new Exception(error);

        public static Result<string> Ok()
            => Ok(OkValue);

        public static Result<string> Ok(string value)
            => new Result<string>(value);

        public static Result<string> Fail(string errorMessage)
            => new Result<string>(Exception(errorMessage));
    }
}
