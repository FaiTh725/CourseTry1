using CourseTry1.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTry1.Dal.Configuration
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasKey(k => k.Id);/*

            builder.HasOne(x => x.DayWeek).WithMany(x => x.PairingTime);*/
        }
    }
}
