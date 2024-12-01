using ArtigosCientificos.Api.Models.User;
using Microsoft.EntityFrameworkCore;


namespace ArtigosCientificos.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User>? Users { get; set; }
    }
}
