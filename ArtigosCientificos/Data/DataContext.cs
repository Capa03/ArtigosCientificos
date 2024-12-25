using System.Data;
using ArtigosCientificos.Api.Models.Article;
using ArtigosCientificos.Api.Models.Role;
using ArtigosCientificos.Api.Models.Token;
using ArtigosCientificos.Api.Models.User;
using Microsoft.EntityFrameworkCore;


namespace ArtigosCientificos.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User>? Users { get; set; }
        public DbSet<UserRole>? UserRoles { get; set; }
        public DbSet<UserToken>? UserTokens { get; set; }
        public DbSet<Article>? Articles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed roles if not present
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 1, Name = "Researcher" },
                new UserRole { Id = 2, Name = "Reviewer" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
