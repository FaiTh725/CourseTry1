using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Entity;
using CourseTry1.Domain.ViewModels.Group;

namespace CourseTry1.Dal.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext context;

        public GroupRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IQueryable<SheduleGroup> GetGroups()
        {
            return context.SheduleGroups;
        }
    }
}
