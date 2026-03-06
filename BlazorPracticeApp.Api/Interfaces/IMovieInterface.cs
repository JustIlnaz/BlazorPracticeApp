using BlazorPracticeApp.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlazorPracticeApp.Api.Interfaces
{
    public interface IMovieInterface
    {
        Task<IActionResult> GetAllMovies();
        Task<IActionResult> GetMovieById(int id);
        Task<IActionResult> CreateMovie(NewMovieDto newMovie);
        Task<IActionResult> UpdateMovie(int id, UpdateMovieDto updateMovie);
        Task<IActionResult> DeleteMovie(int id);


    }
}
