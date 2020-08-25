using System.Threading.Tasks;
using MyNotes.Application.SharedKernel;
using MyNotes.Core.UserManagement;
using MyNotes.Core.UserManagement.Entities;
using MyNotes.Core.UserManagement.Factories;
using LanguageExt.Common;

namespace MyNotes.Application.UserManagement.Commands
{
    public class CreateUserCommandHandler : CommandHandler<CreateUserCommand>
    {
        private readonly IUserManagementRepository _userManagementRepository;
        private readonly UserFactory _userFactory;

        public CreateUserCommandHandler(IUserManagementRepository userManagementRepository, UserFactory userFactory)
        {
            _userManagementRepository = userManagementRepository;
            _userFactory = userFactory;
        }

        protected override async Task<Result<string>> ExecuteCommand(CreateUserCommand command)
        {
            var existingUser = await _userManagementRepository.GetByIdAsync<User>(command.UserId);
            
            if (existingUser != null) return Ok();

            var user = _userFactory.Create(command.UserId, command.Name, command.Notes); 
            user.Init();
            await _userManagementRepository.SaveAsync(user);
            return Ok();
        }
    }
}
