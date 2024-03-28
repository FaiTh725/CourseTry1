using CourseTry1.Domain.Entity;
using CourseTry1.Domain.ViewModels.Group;

namespace CourseTry1.Dal.Interfaces
{
    public interface IGroupRepository
    {
        IQueryable<SheduleGroup> GetGroups();
    }
}
