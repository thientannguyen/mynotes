using MyNotes.Application.SharedKernel;
using MyNotes.Core.UserManagement.Entities;

namespace MyNotes.Application.UserManagement.Queries
{
    public class GetUserQuery : Query<User>
    {
        public string UserId { get; }

        public GetUserQuery(string userId)
        {
            UserId = userId;
        }
    }
}