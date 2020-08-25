using System.Collections.Generic;
using MyNotes.Core.SharedKernel;
using MyNotes.Core.UserManagement.Events;

namespace MyNotes.Core.UserManagement.Entities
{
    public class User : AggregateRoot
    {
        public string Name { get; set; }
        public IList<Note> Notes { get; set; }

        public User(string id, string name, IList<Note> notes) : base(id)
        {
            Name = name;
            Notes = notes;
        }

        public User(string id) : base(id)
        {

        }

        public void Init()
        {
            AddEvent(new UserCreated(id, Notes));
        }
    }
}

