using System.Collections.Generic;
using MyNotes.Core.UserManagement.Entities;

namespace MyNotes.Core.UserManagement.Factories
{
    public class UserFactory
    {
        public virtual User Create(string id, string name, IList<Note> notes)
        {
            return new User(id, name, notes);
        }
    }
}
