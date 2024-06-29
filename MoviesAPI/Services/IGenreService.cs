namespace MoviesAPI.Services
{
    public interface IGenreService
    {
        IEnumerable<Genre> GetAll();
        Genre? GetById(byte id);
        Genre Add(Genre genre);
        Genre Update(Genre genre);
        Genre Delete(Genre genre);
        bool IsValidGenre(byte id);
    }
}
