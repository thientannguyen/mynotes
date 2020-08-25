using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyNotes.Application.UserManagement.Commands;
using MyNotes.Application.SharedKernel;
using MyNotes.Core.UserManagement;
using MyNotes.Core.UserManagement.Entities;
using MyNotes.Core.UserManagement.Factories;
using NSubstitute;
using Xunit;

namespace MyNotes.Application.UserManagement.UnitTests.Commands
{
    public class CreateUserCommandHandlerShould
    {
        private readonly CreateUserCommandHandler _sut;
        private readonly IUserManagementRepository _repository;
        private readonly UserFactory _userFactory;
        private const string UserId = "UserId";
        private const string Name = "David";
        private readonly IList<Note> _notes = new List<Note>
        {
            new Note("id1", "note1"),
            new Note("id2", "note2")
        };

        public CreateUserCommandHandlerShould()
        {
            _repository = Substitute.For<IUserManagementRepository>();
            _userFactory = Substitute.For<UserFactory>();
            _sut = new CreateUserCommandHandler(_repository, _userFactory);
        }

        [Fact]
        public void BeACommandHandler()
        {
            Assert.IsAssignableFrom<CommandHandler<CreateUserCommand>>(_sut);
        }

        [Fact]
        public async Task BeAbleToSaveUserWhenCreateUser()
        {
            // Arrange
            var command = new CreateUserCommand(UserId, Name, _notes);
            var user = Substitute.ForPartsOf<User>(UserId, Name, _notes);
            _userFactory.Create(UserId, Name, _notes).Returns(user);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _userFactory.Received().Create(Arg.Is(UserId), Arg.Is(Name), Arg.Is(_notes));
            user.Received().Init();
            await _repository.Received().SaveAsync(Arg.Is<User>(p => p.id == UserId && p.Name == Name && p.Notes.SequenceEqual(_notes)));
        }

        [Fact]
        public async Task NotSaveUserWhenUserIsAlreadyExist()
        {
            // Arrange
            var command = new CreateUserCommand(UserId, Name, _notes);
            var user = Substitute.ForPartsOf<User>(UserId, Name, _notes);
            _repository.GetByIdAsync<User>(UserId).ReturnsForAnyArgs(user);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _userFactory.DidNotReceive().Create(Arg.Is(UserId), Arg.Is(Name), Arg.Is(_notes));
            user.DidNotReceive().Init();
            await _repository.DidNotReceive().SaveAsync(Arg.Is<User>(p => p.id == UserId && p.Name == Name && p.Notes.SequenceEqual(_notes)));
        }
    }
}
