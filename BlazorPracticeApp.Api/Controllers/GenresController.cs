using BlazorPracticeApp.Api.ContextDatabase;
using BlazorPracticeApp.Api.DTOs;
using BlazorPracticeApp.Api.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPracticeApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly ContextDb context;

        public GenresController(ContextDb context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await context.Genres.Where(g => g.NameGenre != null && g.NameGenre != "").OrderBy(g => g.NameGenre).Select(g => g.NameGenre!).ToListAsync();

            return new OkObjectResult(new
            {
                status = true,
                list = genres
            });
        }

        [HttpPost]
        [RoleAutorizeAttribute([1])]
        public async Task<IActionResult> CreateGenre([FromBody] CreateGenreDto dto)
        {
            var name = (dto.Name ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new
                {
                    status = false,
                    message = "Название жанра не может быть пустым"
                });
            }

            var nameLower = name.ToLower();
            var exists = await context.Genres.AnyAsync(g => g.NameGenre != null && g.NameGenre.ToLower() == nameLower);
            if (exists)
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = "Такой жанр уже существует"
                });
            }

            context.Genres.Add(new Models.Genre { NameGenre = name });
            await context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true,
                message = "Жанр добавлен",
                genre = name
            });
        }
    }
}

