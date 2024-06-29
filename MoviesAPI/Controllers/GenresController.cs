using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Services;


namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService) 
        {
            _genreService = genreService;
        }

        [HttpGet]
        public IActionResult GetAllGenres()
        {
            var GenresList = _genreService.GetAll();
            return Ok(GenresList);
        }

        [HttpPost]
        public IActionResult CreateGenre(GenreDTO genreDTO)
        {
            Genre genre = new Genre(){ Name = genreDTO.Name };
            _genreService.Add(genre);
            return Ok(genre);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGenre(byte id, [FromBody] GenreDTO genreDTO)
        {
            var genre = _genreService.GetById(id);
            if(genre is not null)
            {
                genre.Name = genreDTO.Name;
                _genreService.Update(genre);
                return Ok(genre);
            }
            return NotFound($"No Genre was found with ID: {id}");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGenre(byte id)
        {
            var genre = _genreService.GetById(id);
            if(genre is not null)
            {
                _genreService.Delete(genre);
                return Ok(genre);
            }
            return NotFound($"No Genre was found with ID: {id}");
        }
    }
}
