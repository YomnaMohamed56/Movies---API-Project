
namespace MoviesAPI.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;

        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetAll()
        {
            return _context.Genres.OrderBy(g => g.Name).ToList();
        }

        public Genre? GetById(byte id)
        {
            return _context.Genres.SingleOrDefault(g => g.Id == id);
        }

        public Genre Add(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
            return genre;
        }

        public Genre Update(Genre genre)
        {
            _context.Genres.Update(genre);
            _context.SaveChanges();
            return genre;
        }

        public Genre Delete(Genre genre)
        {
            _context.Genres.Remove(genre);
            _context.SaveChanges();
            return genre;
        }     

        public bool IsValidGenre(byte id)
        {
            return _context.Genres.Any(g => g.Id == id);
        }
    }
}
