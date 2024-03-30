using CourseTry1.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTry1.Dal.Configuration
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(x => x.Id);

            /*builder.HasOne(x => x.User).WithOne(x => x.Profile);

            builder.HasMany(x => x.SheduleGroup).WithMany(x => x.Profiles);*/
        }
    }
}
