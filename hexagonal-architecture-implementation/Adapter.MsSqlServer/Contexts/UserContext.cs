using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Adapter.MsSqlServer.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> context) : base(context)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserState> UserStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();

            modelBuilder.Entity<UserState>().HasData(new UserState { Id = (int)State.Active, Code = State.Active, Description = "Active User State" });
            modelBuilder.Entity<UserState>().HasData(new UserState { Id = (int)State.Blocked, Code = State.Blocked, Description = "Blocked User State" });

            modelBuilder.Entity<UserGroup>().HasData(new UserGroup { Id = (int)Group.User, Code = Group.User, Description = "Default User Group" });
            modelBuilder.Entity<UserGroup>().HasData(new UserGroup { Id = (int)Group.Admin, Code = Group.Admin, Description = "Admin User Group" });

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Login = "adminLogin",
                    PasswordHash = "adminHashedPass",
                    UserGroupId = (int)Group.Admin,
                    UserStateId = (int)State.Active,
                });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Login = "userLogin",
                PasswordHash = "userHashedPass",
                UserGroupId = (int)Group.User,
                UserStateId = (int)State.Active,
            });
        }
    }
}
