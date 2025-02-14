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


            modelBuilder.Entity<SystemProfile>()
        .HasOne(p => p.Profile)
        .WithMany(p => p.Children)
        .HasForeignKey(p => p.ProfileId)
        .OnDelete(DeleteBehavior.Restrict); // Prevents cascading delete loop

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

        public DbSet<SystemProfile> systemProfiles { get; set; }

        public DbSet<Audit> auditLogs { get; set; }

        public virtual async Task<int> SaveChangesAsync(string userId = null)
        {
            OnBeforeSavingChanges(userId);
            var result = await base.SaveChangesAsync();
            return result;
        }

        private void OnBeforeSavingChanges(string userId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach(var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged) continue;

                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntries.Add(auditEntry);

                foreach( var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if(property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch(entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Modified:
                            if(property.IsModified) 
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            
                            break;
                    }
                }
                  
            }

            foreach ( var auditentry in auditEntries )
            {
                auditLogs.Add(auditentry.ToAudit());
            }
        }

    }
}
