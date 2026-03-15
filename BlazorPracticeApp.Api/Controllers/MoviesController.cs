using BlazorPracticeApp.Api.DTOs;
using BlazorPracticeApp.Api.Interfaces;
using BlazorPracticeApp.Api.JWT;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace BlazorPracticeApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService service;

        public MoviesController(IMovieService _service)
        {
            service = _service;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            return await service.GetAllMovies();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieId(int id)
        {
            return await service.GetMovieById(id);
        }


        [HttpPost]
        [RoleAutorizeAttribute([1])]
        public async Task<IActionResult> CreateNewMovie(NewMovieDto newMovieDto)
        {
            return await service.CreateMovie(newMovieDto);
        }


        [HttpPut("{id}")]
        [RoleAutorizeAttribute([1])]
        public async Task<IActionResult> UpdateMovie(int id, UpdateMovieDto updateMovieDto)
        {
            return await service.UpdateMovie(id, updateMovieDto);
        }

        [HttpDelete("{id}")]
        [RoleAutorizeAttribute([1])]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            return await service.DeleteMovie(id);
        }



    }


}
