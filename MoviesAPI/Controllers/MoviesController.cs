using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;
using MoviesAPI.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private new List<string> _allowedExtenstions = new List<string>() { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1024 * 1024;

        private readonly IMoviesService _moviesService;
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;

        public MoviesController(IMoviesService moviesService, IGenreService genreService, IMapper mapper)
        {
            _moviesService = moviesService;
            _genreService = genreService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllMovies()
        {
            #region//First way:
            //var movies = _context.Movies
            //                     .OrderByDescending(m => m.Rate)
            //                     .Include(m => m.Genre)
            //                     .Select(m => new MovieDetailsDTO
            //                     {
            //                         Id = m.Id,
            //                         GenreId = m.GenreId,
            //                         GenreName = m.Genre.Name,
            //                         Title = m.Title,
            //                         Year = m.Year,
            //                         Rate = m.Rate,
            //                         Storeline = m.Storeline,
            //                         Poster = m.Poster
            //                     })
            //                     .ToList();
            #endregion

            //Second way:
            var movies = _moviesService.GetAll();
            
            //mapping movies to MovieDetailsDTO using auto mapper
            var data = _mapper.Map<IEnumerable<MovieDetailsDTO>>(movies);

            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var movie = _moviesService.GetById(id);
            if (movie == null)
                return NotFound();

            #region//Mapping data from Movie to MovieDetailsDTO using regular way
            //var MovieDetailsDTO = new MovieDetailsDTO
            //{
            //    Id = movie.Id,
            //    GenreId = movie.GenreId,
            //    GenreName = movie.Genre.Name,
            //    Title = movie.Title,
            //    Year = movie.Year,
            //    Rate = movie.Rate,
            //    Storeline = movie.Storeline,
            //    Poster = movie.Poster
            //};
            #endregion

            //Mapping data from Movie to MovieDetailsDTO using autoMapper
            var data = _mapper.Map<MovieDetailsDTO>(movie);

            return Ok(data);
        }

        [HttpGet("GetByGenreId")]
        public IActionResult GetByGenreId(byte GenreiId)
        {
            var movies = _moviesService.GetAll(GenreiId);

            //mapping movies to MovieDetailsDTO using auto mapper
            var data = _mapper.Map<IEnumerable<MovieDetailsDTO>>(movies);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromForm] MovieDTO movieDTO)
        {
            if (movieDTO.Poster is null)
                return BadRequest("Poster is required!");

            if (!_allowedExtenstions.Contains(Path.GetExtension(movieDTO.Poster.FileName).ToLower()))
                return BadRequest("Only .jpg and .png images are allowed!");

            if (movieDTO.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            bool isValidGenre = _genreService.IsValidGenre(movieDTO.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid genre id!");

            using var dataStream = new MemoryStream();
            await movieDTO.Poster.CopyToAsync(dataStream);

            #region//Mapping data from MovieDTO to Movie using regular way
            //Movie movie = new Movie()
            //{
            //    Title = movieDTO.Title,
            //    Year = movieDTO.Year,
            //    Rate = movieDTO.Rate,
            //    Storeline = movieDTO.Storeline,
            //    Poster = dataStream.ToArray(),
            //    GenreId = movieDTO.GenreId,
            //};
            #endregion

            //Mapping data from Movie to MovieDetailsDTO using autoMapper
            Movie movie = _mapper.Map<Movie>(movieDTO);
            movie.Poster = dataStream.ToArray();

            _moviesService.Add(movie);
            return Ok(movie);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromForm]MovieDTO movieDTO)
        {
            var movie = _moviesService.GetById(id);

            if (movie == null)
                return NotFound($"No movie was found with id: {id}");

            bool isValidGenre = _genreService.IsValidGenre(movieDTO.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid genre id!");

            if(movieDTO.Poster is not null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(movieDTO.Poster.FileName).ToLower()))
                    return BadRequest("Only .jpg and .png images are allowed!");

                if (movieDTO.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");

                using var dataStream = new MemoryStream();
                await movieDTO.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }

            movie.Title = movieDTO.Title;
            movie.Year = movieDTO.Year;
            movie.Rate = movieDTO.Rate;
            movie.Storeline = movieDTO.Storeline;   
            movie.GenreId = movieDTO.GenreId;

            _moviesService.Update(movie);

            return Ok(movie);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteMovie(int id)
        {
            var movie = _moviesService.GetById(id);

            if (movie == null)
                return NotFound($"No movie was found with id: {id}");

            _moviesService.Delete(movie);
            return Ok(movie);
        }
    }
}
