using System.Threading.Tasks;
using MyNotes.Application.SharedKernel;
using MyNotes.Core.UserManagement;
using MyNotes.Core.UserManagement.Entities;

namespace MyNotes.Application.UserManagement.Queries
{
    public class GetUserQueryHandler : QueryHandler<GetUserQuery, User> {

        private readonly IUserManagementRepository _repository;

        public GetUserQueryHandler(IUserManagementRepository repository)
        {
            _repository = repository;
        }

        protected override async Task<User> ExecuteQuery(GetUserQuery query)
        {
            return await _repository.GetByIdAsync<User>(query.UserId);
        }
    }
}