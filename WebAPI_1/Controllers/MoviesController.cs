using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_1.Data;
using WebAPI_1.Dtos;
using WebAPI_1.Models;

namespace WebAPI_1.Controllers
{
	//[Route("/")]
	[Route("api/[controller]")]
	[ApiController]
	public class MoviesController : ControllerBase
	{
		private readonly ApplicationDBContext _context;

		private List<string> allowedExtensions = new List<string> { ".PNG", ".JPG", ".GIF", ".TIFF" };

		private long maxPosterSizeAllowed = 1097152;
		public MoviesController(ApplicationDBContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var Movies = await _context.Movies.Include(e => e.Genre).ToListAsync();
			//var vv = await _context.Movies.Select(e => new
			//{
			//	e.Id,
			//	e.Title,
			//	e.Year,
			//	e.rate,
			//	e.StoryLine,
			//	e.GenreId,
			//	e.Genre.Name
			//})
			//	.ToListAsync();
			return Ok(Movies);
		}

		[HttpGet("{Id}")]
		public async Task<IActionResult> GetByIdAsync(int Id)
		{
			var selected = await _context.Movies
				.Include(e => e.Genre)
				.SingleOrDefaultAsync(e => e.Id == Id);

			if (selected is null)
				return NotFound("Invalid Movie ID");

			return Ok(selected);
		}

		[HttpGet("GetMoviesByGenreId/{Id}")]
		public async Task<IActionResult> GetMoviesByGenreId(int Id)
		{
			var movies = await _context.Movies
				.Where(e => e.GenreId == Id)
				.Include(e => e.Genre)
				.ToListAsync();

			if (movies is null)
				return NotFound("Invalid Genre ID");

			return Ok(movies);
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromForm] MovieDto movieDto)
		{
			if (movieDto.Poster == null)
				return BadRequest("Poster is Required");

			var PosterExtension = Path.GetExtension(movieDto.Poster.FileName).ToUpper();

			if (!allowedExtensions.Contains(PosterExtension))
				return BadRequest("Only PNG , JPG , GIF , TIFF Allowed");

			if (movieDto.Poster.Length > maxPosterSizeAllowed)
				return BadRequest("Maximum photo size allowed is 1MB");

			var isValidGenre = await _context.Genres.AnyAsync(e => e.Id == movieDto.GenreId);

			if (!isValidGenre)
				return BadRequest("Invalid genre Id!");

			using var datastream = new MemoryStream();

			await movieDto.Poster.CopyToAsync(datastream);

			var movie = new Movie
			{
				Title = movieDto.Title,
				Year = movieDto.Year,
				rate = movieDto.rate,
				Poster = datastream.ToArray(),
				StoryLine = movieDto.StoryLine,
				GenreId = movieDto.GenreId
			};

			await _context.Movies.AddAsync(movie);

			_context.SaveChanges();

			return Ok(movie);
		}

		[HttpDelete("{Id}")]
		public async Task<IActionResult> Delete(int Id)
		{
			var movie =await _context.Movies.FindAsync(Id);

			if (movie is null)
				return NotFound("Invalid ID");

			_context.Remove(movie);
			_context.SaveChanges();
			return Ok(movie);
		}

		[HttpPut("{Id}")]
		public async Task<IActionResult> Update(int Id, [FromForm] MovieDto movieDto)
		{
			var movie = await _context.Movies.FindAsync(Id);

			if (movie == null)
				return BadRequest("Invalid Movie ID");

			var validateGenreId = await _context.Genres
				.AnyAsync(e => e.Id == movieDto.GenreId);

			if (!validateGenreId)
				return BadRequest("Invalid Genre ID");

			if (movieDto.Poster !=null)
			{
				var PosterExtension = Path.GetExtension(movieDto.Poster.FileName).ToUpper();

				if (!allowedExtensions.Contains(PosterExtension))
					return BadRequest("Only PNG , JPG , GIF , TIFF Allowed");

				if (movieDto.Poster.Length > maxPosterSizeAllowed)
					return BadRequest("Maximum photo size allowed is 1MB");
			}

			movie.Title = movieDto.Title;
			movie.rate = movieDto.rate;
			movie.StoryLine = movieDto.StoryLine;
			movie.Year = movieDto.Year;
			movie.GenreId = movieDto.GenreId;

			_context.SaveChanges();
			return Ok(movie);
		}


	}
}
