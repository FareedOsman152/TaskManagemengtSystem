using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManagmentSystem.Models
{ 
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<WorkSpace> WorkSpaces { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AppUserProfile> AppUserProfiles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamAppUser> TeamAppUser { get; set; }
        public DbSet<TaskEdiotor> TaskEdiotor { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserTask>().HasKey(x => x.Id);
            builder.Entity<UserTask>().Property(x => x.Title).HasMaxLength(50);
            builder.Entity<UserTask>().Property(x=>x.Description).HasMaxLength(100);
            builder.Entity<UserTask>().Property(x => x.BeginOn).IsRequired(false);
            builder.Entity<UserTask>().Property(x => x.EndOn).IsRequired(false);
            builder.Entity<UserTask>(e =>
            {
                e.Property(x => x.RemindMeBeforeBegin).IsRequired(false);
                e.Property(x => x.RemindMeBeforeEnd).IsRequired(false);
                e.Property(x => x.IsDone).HasDefaultValue(false);

                e.HasOne(t => t.Creater)
               .WithMany(u => u.TasksCreated)
               .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(t => t.Editors)
                .WithMany(u => u.TasksEdited)
                .UsingEntity<TaskEdiotor>();

            });

            builder.Entity<TaskList>().HasKey(x => x.Id);
            builder.Entity<TaskList>().Property(x => x.Title).HasMaxLength(50);
            builder.Entity<TaskList>().Property(x => x.Description).HasMaxLength(100);

            builder.Entity<WorkSpace>().HasKey(x => x.Id);
            builder.Entity<WorkSpace>().Property(x => x.Title).HasMaxLength(50);
            builder.Entity<WorkSpace>().Property(x => x.Description).HasMaxLength(100);

            builder.Entity<Notification>().HasKey(x => x.Id);
            builder.Entity<Notification>().Property(x => x.Details).HasMaxLength(200);
            builder.Entity<Notification>()
                .HasOne(x=>x.UserTask)
                .WithMany(x=>x.Notifications)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Notification>()
                .HasOne(x => x.AppUser)
                .WithMany(x => x.Notifications)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AppUserProfile>(p =>
            {
                p.HasKey(p => p.Id);
                p.Property(p=>p.FirstName).HasMaxLength(30);    
                p.Property(p=>p.LastName).HasMaxLength(30);    
                p.HasOne(x => x.AppUser)
                .WithOne(u => u.Profile)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Team>(t =>
            {
                t.Property(t => t.Title).HasMaxLength(75);
                t.Property(t => t.Description).HasMaxLength(255);
                t.HasOne(t => t.Admin)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

                t.HasMany(t => t.Users)
                .WithMany(u => u.Teams)
                .UsingEntity<TeamAppUser>();

                t.HasMany(t => t.WorkSpaces)
                .WithOne(w => w.Team)
                .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<TaskEdiotor>(te =>
            {
                te.HasKey(te => new { te.EditorId, te.TaskEditedId });

                te.HasOne(te => te.Editor)
                .WithMany(u => u.TaskEditor)
                .HasForeignKey(te => te.EditorId)
                .OnDelete(DeleteBehavior.Restrict);

                te.HasOne(te => te.TaskEdited)
                .WithMany(u => u.TaskEditor)
                .HasForeignKey(te => te.TaskEditedId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
