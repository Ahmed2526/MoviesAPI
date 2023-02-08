using System.ComponentModel.DataAnnotations;

namespace WebAPI_1.Dtos
{
	public class MovieDto
	{
		public string Title { get; set; }
		public int Year { get; set; }
		public double rate { get; set; }

		[MaxLength(2500)]
		public string StoryLine { get; set; }
		public IFormFile? Poster { get; set; }
		public int GenreId { get; set; }
	}
}
