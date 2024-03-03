using CourseTry1.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace CourseTry1.Dal
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
