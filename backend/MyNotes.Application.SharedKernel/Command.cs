namespace MyNotes.Application.SharedKernel
{
    using LanguageExt.Common;
    using MediatR;

    public abstract class Command : IRequest<Result<string>>
    {
    }
}
