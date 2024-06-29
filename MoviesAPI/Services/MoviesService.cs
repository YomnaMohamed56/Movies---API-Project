
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly ApplicationDbContext _context;

        public MoviesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Movie> GetAll(byte genreId = 0)
        {
            return _context.Movies
                           .Where(m => m.GenreId == genreId || genreId == 0)
                           .OrderByDescending(m => m.Rate)
                           .Include(m => m.Genre)
                           .ToList();
        }

        public Movie? GetById(int id)
        {
            return _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);
        }

        public Movie Add(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie Update(Movie movie)
        {
            _context.Movies.Update(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie Delete(Movie movie)
        {
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return movie;
        }     
    }
}
