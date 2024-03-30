using CourseTry1.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTry1.Dal.Configuration
{
    public class SheduleGroupConfiguration : IEntityTypeConfiguration<SheduleGroup>
    {
        public void Configure(EntityTypeBuilder<SheduleGroup> builder)
        {
            builder.HasKey(x => x.Id);

            /*builder.HasMany(f => f.Weeks).WithOne(x => x.SheduleGroup).HasForeignKey(x => x.GroupId);
            builder.HasMany(f => f.Profiles).WithMany(x => x.SheduleGroup);*/
        }
    }
}
