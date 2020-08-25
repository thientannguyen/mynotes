using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyNotes.API.UserManagement;
using MyNotes.Application.UserManagement.Commands;
using MyNotes.Application.UserManagement.Queries;
using MyNotes.Core.UserManagement.Entities;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace MyNotes.API.UnitTests
{
    public class UserManagementControllerShould
    {
        private readonly UserManagementController _sut;
        private readonly IMediator _mediator;
        private const string UserId = "UserId";
        private const string Name = "David";
        private User _user;
        private readonly IList<Note> _notes = new List<Note>
        {
            new Note("id1", "note1"),
            new Note("id2", "note2")
        };

        public UserManagementControllerShould()
        {
            _mediator = Substitute.For<IMediator>();
            _sut = new UserManagementController(_mediator);
            StubUser();
        }

        private void StubUser()
        {
            _user = Substitute.ForPartsOf<User>(UserId, Name, _notes);
        }

        private static UserManagementController.UserDto CreateUserDto()
        {
            return new UserManagementController.UserDto
            {
                Id = UserId,
                Name = Name,
                Notes = new[]
                {
                    new UserManagementController.NoteDto
                    {
                        Id = "id1",
                        Title = "note1"
                    },
                    new UserManagementController.NoteDto
                    {
                        Id = "id2",
                        Title = "note2"
                    }

                }
            };
        }

        [Fact]
        public async Task ReturnTrueForSucceededCreateUser()
        {
            _mediator.Send(Arg.Any<CreateUserCommand>()).Returns("");

            var result = await _sut.CreateUser(CreateUserDto());

            Assert.IsType<OkObjectResult>(result);
            await _mediator.Received().Send(Arg.Is<CreateUserCommand>(cmd =>
                cmd.UserId == UserId && cmd.Name == Name &&
                cmd.Notes.SequenceEqual(_notes, new NoteComparer())));
        }

        [Fact]
        public async Task ReturnsFalseForCreateUserError()
        {
            _mediator.Send(Arg.Any<CreateUserCommand>()).Returns(new Result<string>(new ArgumentException("")));

            var result = (ObjectResult)await _sut.CreateUser(CreateUserDto());

            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task ReturnNoContentForUserWhichIsNotPresent()
        {
            _mediator.Send(Arg.Any<GetUserQuery>()).Returns(Task.FromResult<User>(null));

            var result = await _sut.GetUser(UserId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ReturnAUser()
        {
            _mediator.Send(Arg.Is<GetUserQuery>(p => p.UserId == UserId)).Returns(_user);

            var result = await _sut.GetUser(UserId);
            var res = (UserManagementController.UserDto)((OkObjectResult)result).Value;

            result.Should().BeOfType<OkObjectResult>()
                .Which
                .Value.Should().BeOfType<UserManagementController.UserDto>();

            Assert.Equal(_user.id, res.Id);
        }

        private class NoteComparer : IEqualityComparer<Note>
        {
            public bool Equals(Note x, Note y)
            {
                if (x == null && y == null) return true;
                if (x == null || y == null) return false;
                if (x.Title != y.Title) return false;

                return true;
            }

            public int GetHashCode(Note obj)
            {
                return 0;
            }
        }
    }
}
