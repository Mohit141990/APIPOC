using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebAPI5.Entities.EntityConfiguration
{
    public class EnrollmentsConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.Property(x => x.EnrollmentID).IsRequired(); 
            builder.Property(x => x.StudentID).IsRequired();
            builder.Property(x => x.CourseName).HasMaxLength(200);
            builder.Property(x => x.CourseCode).HasMaxLength(20);
            builder.Property(x => x.Credits);
            builder.Property(x => x.EnrollmentDate).HasColumnType("datetime");
            builder.Property(x => x.Grade).HasMaxLength(20);
        }
    }
}
