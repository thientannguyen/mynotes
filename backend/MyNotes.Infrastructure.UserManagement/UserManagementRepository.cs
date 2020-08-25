using MyNotes.Core.SharedKernel;
using MyNotes.Core.UserManagement;

namespace MyNotes.Infrastructure.UserManagement
{
    public class UserManagementRepository : Repository, IUserManagementRepository
    {
        public UserManagementRepository(IDomainEventPublisher eventPublisher, LiteDbContext dbContext) : base(eventPublisher, dbContext)
        {
        }
    }
}
