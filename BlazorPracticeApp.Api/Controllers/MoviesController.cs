using BlazorPracticeApp.Api.DTOs;
using BlazorPracticeApp.Api.Interfaces;
using BlazorPracticeApp.Api.JWT;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace BlazorPracticeApp.Api.Controllers
{
    public class MoviesController
    {
        private readonly IMovieService service;

        public MoviesController(IMovieService _service)
        {
            service = _service;
        }

        [HttpGet]
        [Route("/api/movies/GetMovies")]
        public async Task<IActionResult> GetMovies()
        {
            return await service.GetAllMovies();
        }

        [HttpGet]
        [Route("/api/movies/GetMovieById/{id}")]

        public async Task<IActionResult> GetMovieId(int id)
        {
            return await service.GetMovieById(id);
        }


        [HttpPost]
        [Route("/api/movies/CreateMovie")]
        [RoleAutorizeAttribute([1])]
        public async Task<IActionResult> CreateNewMovie(NewMovieDto newMovieDto)
        {
            return await service.CreateMovie(newMovieDto);
        }


        [HttpPut]
        [Route("/api/movies/UpdateMovies/{id}")]
        [RoleAutorizeAttribute([1])]

        public async Task<IActionResult> UpdateMovie(int id, UpdateMovieDto updateMovieDto)
        {
            return await service.UpdateMovie(id, updateMovieDto);
        }

        [HttpDelete]
        [Route("/api/movies/DeleteMovie/{id}")]
        [RoleAutorizeAttribute([1])]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            return await service.DeleteMovie(id);
        }



    }


}
