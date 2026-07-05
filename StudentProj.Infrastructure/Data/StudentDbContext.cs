using Microsoft.EntityFrameworkCore;
using StudentProj.Domain.Entities;
using System.Data;

namespace StudentProj.Data
{
    public class StudentDbcontext : DbContext
    {
        public StudentDbcontext(DbContextOptions<StudentDbcontext> options) : base(options)
        {
        }
        public DbSet<Student> Student { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<StudentRoles> StudentRoles { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<RoutePermissions> RoutePermissions { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentRoles>(entity =>
            {

                entity.HasOne(sr => sr.Student)
                    .WithMany(s => s.StudentRoles)
                    .HasForeignKey(sr => sr.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sr => sr.Role)
                    .WithMany(r => r.StudentRoles)
                    .HasForeignKey(sr => sr.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RolePermissions>(entity =>
            {
                entity.HasOne(rp => rp.Menu)
                    .WithMany(m => m.RolePermissions)
                    .HasForeignKey(rp => rp.MenuId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Roles>().HasData(
                new Roles { Id = 1, RoleName = "Super Admin", IsDeleted = false },
                new Roles { Id = 2, RoleName = "Admin", IsDeleted = false },
                new Roles { Id = 3, RoleName = "User", IsDeleted = false }
            );

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasOne(n => n.Subject)
                .WithMany()
                .HasForeignKey(n => n.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);
            });

            // ===== Filtered Unique Indexes (Enterprise-Level) =====
            // These ensure data integrity at the DB level even under race conditions.
            // Filtered on IsDeleted = 0 so soft-deleted records don't block re-creation.

            // Student: Email must be unique among active records
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .HasFilter("IsDeleted = 0")
                .IsUnique();

            // Student: Phone must be unique among active records
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Phone)
                .HasFilter("IsDeleted = 0")
                .IsUnique();

            // Role: RoleName must be unique among active records
            modelBuilder.Entity<Roles>()
                .HasIndex(r => r.RoleName)
                .HasFilter("IsDeleted = 0")
                .IsUnique();

            // Permission: PermissionName must be unique among active records
            modelBuilder.Entity<Permissions>()
                .HasIndex(p => p.PermissionName)
                .HasFilter("IsDeleted = 0")
                .IsUnique();

            // Menu: MenuName must be unique among active records
            modelBuilder.Entity<Menu>()
                .HasIndex(m => m.MenuName)
                .HasFilter("IsDeleted = 0")
                .IsUnique();

            // Course: CourseName must be unique among active records
            modelBuilder.Entity<Course>()
                .HasIndex(c => c.CourseName)
                .HasFilter("isDeleted = 0")
                .IsUnique();

            // Subject: SubjectCode + CourseId must be unique among active records
            modelBuilder.Entity<Subject>()
                .HasIndex(s => new { s.SubjectCode, s.CourseId })
                .HasFilter("IsDeleted = 0")
                .IsUnique();
        }
    }
}
