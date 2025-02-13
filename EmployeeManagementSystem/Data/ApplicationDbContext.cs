using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Disable cascade delete to prevent multiple cascade paths
            modelBuilder.Entity<LeaveApplication>()
                .HasOne(l => l.Status)
                .WithMany()
                .HasForeignKey(l => l.StatusId)
                .OnDelete(DeleteBehavior.Restrict); // ⬅️ Prevents cascading delete

            modelBuilder.Entity<LeaveApplication>()
                .HasOne(l => l.Duration)
                .WithMany()
                .HasForeignKey(l => l.DurationId)
                .OnDelete(DeleteBehavior.Restrict); // ⬅️ Prevents cascading delete
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Designation> Designations { get; set; }

        public DbSet<SystemCode> SystemCodes { get; set; }

        public DbSet<SystemCodeDetail> systemCodeDetails { get; set; }

        public DbSet<Bank> banks { get; set; }

        public DbSet<LeaveType> leaveTypes { get; set; }

        public DbSet<Country> countries { get; set; }

        public DbSet<City> cities { get; set; }

        public DbSet<LeaveApplication> leaveApplications { get; set; }

    }
}
