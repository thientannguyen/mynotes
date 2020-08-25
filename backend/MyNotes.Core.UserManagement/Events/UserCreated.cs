using System.Collections.Generic;
using MyNotes.Core.UserManagement.Entities;
using MyNotes.Core.SharedKernel;

namespace MyNotes.Core.UserManagement.Events
{
    public class UserCreated : IDomainEvent
    {
        public string Id { get; }
        public IList<Note> Notes { get; }

        public UserCreated(string id, IList<Note> notes)
        {
            Id = id;
            Notes = notes;
        }
    }
}