using CourseTry1.Dal.Configuration;
using CourseTry1.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace CourseTry1.Dal
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ExcelFile> Files { get; set; }
        public DbSet<SheduleGroup> SheduleGroups { get; set; }
        public DbSet<DayWeek> DayWeeks { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SheduleGroupConfiguration());
            modelBuilder.ApplyConfiguration(new DayWeekConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
