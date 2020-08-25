using MyNotes.Core.SharedKernel;

namespace MyNotes.Core.UserManagement.Entities
{
    public class Note : Entity
    {
        public string Title { get; set; }

        public Note(string id, string title) : base(id)
        {
            Title = title;
        }

        public Note(string id) : base(id)
        {

        }
    }
}

