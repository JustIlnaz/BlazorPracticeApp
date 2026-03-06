using BlazorPracticeApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorPracticeApp.Api.ContextDatabase
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(p => p.Email).IsUnique();
        }

    }
}
