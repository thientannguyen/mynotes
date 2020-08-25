
namespace MyNotes.Application.SharedKernel
{
    using MediatR;

    public abstract class Query<T> : IRequest<T>
    {
    }
}
