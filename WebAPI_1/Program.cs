using Microsoft.EntityFrameworkCore;
using WebAPI_1.Data;
using WebAPI_1.Controllers;

namespace WebAPI_1
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<ApplicationDBContext>(opt =>
			opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerUI();
			}

			app.UseSwagger();

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();


			app.Run();
		}
	}
}