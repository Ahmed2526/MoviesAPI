using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_1.Models
{
	public class Movie
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int Year { get; set; }
		public double rate { get; set; }

		[MaxLength(2500)]
		public string StoryLine { get; set; }
		public byte[] Poster { get; set; }
		public int GenreId { get; set; }

		[ForeignKey("GenreId")]
		public Genre Genre { get; set; }

	}
}
