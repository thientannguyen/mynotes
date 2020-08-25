using System.Collections.Generic;
using MyNotes.Application.SharedKernel;
using MyNotes.Core.UserManagement.Entities;

namespace MyNotes.Application.UserManagement.Commands
{
    public class CreateUserCommand : Command
    {
        public string UserId { get; }
        public string Name { get; }
        public IList<Note> Notes { get; }

        public CreateUserCommand(string userId, string name, IList<Note> notes)
        {
            UserId = userId;
            Name = name;
            Notes = notes;
        }
    }
}