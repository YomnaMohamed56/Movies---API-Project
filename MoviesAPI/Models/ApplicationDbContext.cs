using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base() 
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
