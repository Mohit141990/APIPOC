using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI5.Entities;

namespace WebAPI5.Database
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Enrollment> Enrollments { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=MOHITSHARMA\\SQLEXPRESS2019;Initial Catalog=web-api-db; Integrated Security=True; TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
            modelBuilder.Entity<Student>().HasIndex(s => s.StudentID);
            modelBuilder.Entity<Student>().HasIndex(s => new { s.FirstName, s.LastName });
            modelBuilder.Entity<Enrollment>().HasIndex(s => s.EnrollmentID);
            base.OnModelCreating(modelBuilder);
        }

    }
}
