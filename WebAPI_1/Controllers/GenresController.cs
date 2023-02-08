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
	public class GenresController : ControllerBase
	{
		private readonly ApplicationDBContext _context;

		public GenresController(ApplicationDBContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var genres = await _context.Genres.ToListAsync();

			if (genres is null)
			{
				return NotFound();
			}
			return Ok(genres);
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync(GenreDto gen)
		{
			var genre = new Genre() { Name = gen.Name };

			if (genre is null)
			{
				return BadRequest();
			}
			await _context.Genres.AddAsync(genre);
			_context.SaveChanges();

			return Ok(genre);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAsync(int id, [FromBody] GenreDto genre)
		{
			var selected = await _context.Genres.FindAsync(id);

			if (selected is null)
			{
				return NotFound($"Incorrect id {id}");
			}

			selected.Name = genre.Name;
			_context.SaveChanges();

			return Ok(selected);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			var genre = await _context.Genres.FindAsync(id);

			if (genre is null)
			{
				return NotFound($"Incorrect id {id}");
			}

			_context.Genres.Remove(genre);
			_context.SaveChanges();

			return Ok(genre);

		}

	}
}
