using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CourseTry1.Dal.Repositories
{
    public class AccountRepository : IAccountRepository<User>
    {
        private readonly AppDbContext context;
        public AccountRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Add(User entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public IQueryable<User> GetAll()
        {
            return context.Users;
        }
    }
}
