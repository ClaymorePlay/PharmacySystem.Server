using Microsoft.EntityFrameworkCore;
using Plugins.User.Data.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Plugins.User.Data
{
    public class UserDataContext : DbContext
    {
        public DbSet<Models.User> Users { get; set; }

        public DbSet<Session> Sessions { get; set; }


        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Models.User>().HasKey(c => c.Id);
            builder.Entity<Models.User>().HasMany(c => c.Sessions).WithOne(c => c.User).HasForeignKey(c => c.UserId);

            builder.Entity<Session>().HasKey(c => c.Id);

            
        }
    }
}