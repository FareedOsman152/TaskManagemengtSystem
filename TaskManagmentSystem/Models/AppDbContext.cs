using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManagmentSystem.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserTask>().HasKey(x => x.Id);
            builder.Entity<UserTask>().Property(x => x.Title).HasMaxLength(50);
            builder.Entity<UserTask>().Property(x=>x.Discription).HasMaxLength(100);

            builder.Entity<TaskList>().HasKey(x => x.Id);
            builder.Entity<TaskList>().Property(x => x.Title).HasMaxLength(50);
            builder.Entity<TaskList>().Property(x => x.Discription).HasMaxLength(100);

            builder.Entity<WorkSpace>().HasKey(x => x.Id);
            builder.Entity<WorkSpace>().Property(x => x.Title).HasMaxLength(50);
            builder.Entity<WorkSpace>().Property(x => x.Discription).HasMaxLength(100);
        }
    }
}
