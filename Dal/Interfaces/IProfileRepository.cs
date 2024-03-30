using CourseTry1.Domain.Entity;

namespace CourseTry1.Dal.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile> AddGroup(User user, SheduleGroup sheduleGroup);

        Task AddProfileUser(User user);

        
        Task<IEnumerable<SheduleGroup>> GetSelectedGroup(User user);

    }
}
