
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyNotes.Application.UserManagement.Queries;
using MyNotes.Core.UserManagement;
using MyNotes.Core.UserManagement.Entities;
using NSubstitute;
using Xunit;

namespace MyNotes.Application.UserManagement.UnitTests.Queries
{
    public class GetUserQueryHandlerShould
    {
        private readonly GetUserQueryHandler _sut;
        private readonly IUserManagementRepository _repository;
        private const string UserId = "UserId";
        private const string Name = "David";
        private readonly IList<Note> _notes = new List<Note>
        {
            new Note("id1", "note1"),
            new Note("id2", "note2")
        };

        public GetUserQueryHandlerShould()
        {
            _repository = Substitute.For<IUserManagementRepository>();
            _sut = new GetUserQueryHandler(_repository);
        }

        [Fact]
        public async Task ReturnNullForNoUserPresent()
        {
            var result = await _sut.Handle(new GetUserQuery(UserId), new CancellationToken());
            Assert.Null(result);
        }

        [Fact]
        public async Task ReturnAUser()
        {
            var user = Substitute.ForPartsOf<User>(UserId, Name, _notes);

            _repository.GetByIdAsync<User>(UserId).Returns(user);

            var result = await _sut.Handle(new GetUserQuery(UserId), new CancellationToken());

            Assert.Equal(Name, result.Name);
            Assert.Equal(user.id, result.id);
            Assert.True(_notes.SequenceEqual(result.Notes));
        }
    }
}