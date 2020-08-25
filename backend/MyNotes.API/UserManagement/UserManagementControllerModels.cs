namespace MyNotes.API.UserManagement
{
    public partial class UserManagementController
    {
        public class UserDto
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public NoteDto[] Notes { get; set; }
        }

        public class NoteDto
        {
            public string Id { get; set; }
            public string Title { get; set; }
        }
    }
}
