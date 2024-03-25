using CourseTry1.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTry1.Dal.Configuration
{
    public class DayWeekConfiguration : IEntityTypeConfiguration<DayWeek>
    {
        public void Configure(EntityTypeBuilder<DayWeek> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(f => f.PairingTime).
                WithOne(f => f.DayWeek).
                OnDelete(DeleteBehavior.Cascade);
        }
    }
}
