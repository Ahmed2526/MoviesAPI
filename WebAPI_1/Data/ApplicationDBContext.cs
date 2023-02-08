using Microsoft.EntityFrameworkCore;
using WebAPI_1.Models;

namespace WebAPI_1.Data
{
	public class ApplicationDBContext : DbContext
	{
		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
		{
		
		}

		public DbSet<Genre> Genres { get; set; }
		public DbSet<Movie> Movies { get; set; }
	}
}
