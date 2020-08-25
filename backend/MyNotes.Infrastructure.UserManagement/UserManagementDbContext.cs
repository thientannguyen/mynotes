using LiteDB;
using MyNotes.Core.UserManagement.Entities;

namespace MyNotes.Infrastructure.UserManagement
{
    public class UserManagementDbContext : LiteDbContext
    {
        public UserManagementDbContext(ConnectionString config) : base(config)
        {
        }

        public override string ContainerName => "User";
        protected override void CreateModel()
        {
            Database.Mapper.Entity<User>()
                .Id(user => user.id)
                .Ignore(user => user.Events);
            Database.Mapper.Entity<Note>()
                .Id(note => note.id);
        }
    }
}
