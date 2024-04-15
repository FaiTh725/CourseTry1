using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace CourseTry1.Dal.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext context;

        public ProfileRepository(AppDbContext context)
        {
            this.context = context;
        }


        public async Task<Profile> AddGroup(User user, SheduleGroup sheduleGroup)
        {
            var profile = await GetProfileByUserId(user);

            profile.Groups.Add(sheduleGroup);

            await context.SaveChangesAsync();

            return profile;
        }

        public async Task AddProfileUser(User user)
        {
            var profile = context.Profiles
                .Include(x => x.User)
                .Include(x => x.Groups).FirstOrDefault(p => p.UserId == user.Id);

            if (profile == null)
            {
                context.Profiles.Add(new Profile()
                {
                    User = user,
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task<Profile> DeleteGroup(User user, SheduleGroup sheduleGroup)
        {
            var profile = await GetProfileByUserId(user);

            profile?.Groups.Remove(sheduleGroup);

            await context.SaveChangesAsync();

            return profile;
        }

        public async Task<Profile> GetProfileByUserId(User user)
        {
            return await context.Profiles
                .Include(x => x.Groups)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == user.Id);
        }

        public async Task<IEnumerable<SheduleGroup>> GetSelectedGroup(User user)
        {
            var profile = await context.Profiles.Include(x => x.User)
                .Include(x => x.Groups)
                .FirstOrDefaultAsync(y => y.UserId == user.Id) ?? new Profile();

            return profile.Groups;
        }
    }
}
