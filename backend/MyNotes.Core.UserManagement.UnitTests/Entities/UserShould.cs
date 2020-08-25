using MyNotes.Core.UserManagement.Entities;
using System.Collections.Generic;
using System.Linq;
using MyNotes.Core.UserManagement.Events;
using Xunit;

namespace MyNotes.Core.UserManagement.UnitTests.Entities
{
    public class UserShould
    {
        private const string Id = "UserId";
        private const string Name = "David";

        private static readonly IList<Note> Notes = new List<Note>
        {
            new Note("id1", "note1"),
            new Note("id2", "note2")
        };

        [Fact]
        public void Init()
        {
            //arrange
            var user = new User(Id, Name, Notes);

            //act
            user.Init();

            //assert
            var userCreatedEvent = user.Events.Single(e => e.GetType() == typeof(UserCreated)) as UserCreated;
            Assert.NotNull(userCreatedEvent);
            Assert.Equal(user.id, userCreatedEvent.Id);
            Assert.Equal(Name, user.Name);
            Assert.Equal(Notes, userCreatedEvent.Notes);
        }
    }
}

