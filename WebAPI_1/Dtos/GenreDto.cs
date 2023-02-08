using System.ComponentModel.DataAnnotations;

namespace WebAPI_1.Dtos
{
	public class GenreDto
	{
		[MaxLength(100)]
		public string Name { get; set; }
	}
}
