namespace MoviesAPI.Services
{
    public interface IMoviesService
    {
        IEnumerable<Movie> GetAll(byte genreId = 0);
        Movie? GetById(int id);
        Movie Add(Movie movie);
        Movie Update(Movie movie);
        Movie Delete(Movie movie);
    }
}
