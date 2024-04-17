using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Enum;
using CourseTry1.Domain.ViewModels.Group;
using Microsoft.EntityFrameworkCore;

namespace CourseTry1.Dal.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext context;

        public GroupRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<int>> GetCources()
        {
            return await context.SheduleGroups.GroupBy(x => x.Cource).Select(x => x.First().Cource).ToListAsync();
        }

        public async Task<DayWeek> GetDayByPram(string nameGroup, DayOfWeek dayOfWeek, int cource, Week week)
        {
            string patter = nameGroup[..^2];

            var shedule = await context.DayWeeks.
                Include(x => x.SheduleGroup).
                Include(x => x.PairingTime).
                FirstOrDefaultAsync(x => x.SheduleGroup.NameGroup.Contains(patter)
                && x.SheduleGroup.Cource == cource 
                && x.SheduleGroup.Week == week 
                && x.DayOfWeek == dayOfWeek);

            return shedule;
        }

        public async Task<DayWeek> GetDayWeek(int idGroup, DayOfWeek dayOfWeek)
        {
            return await context.DayWeeks
                .Include(x => x.PairingTime)
                .Include(x => x.SheduleGroup)
                .FirstOrDefaultAsync(x => x.SheduleGroupId == idGroup && x.DayOfWeek == dayOfWeek); 
        }

        public async Task<SheduleGroup> GetGroupById(int id)
        {
            return await context.SheduleGroups.Include(x => x.Weeks).FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<SheduleGroup> GetGroups()
        {
            return context.SheduleGroups;
        }
    }
}
