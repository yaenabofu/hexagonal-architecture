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
        }
    }
}
