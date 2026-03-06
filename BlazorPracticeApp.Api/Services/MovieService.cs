using BlazorPracticeApp.Api.ContextDatabase;
using BlazorPracticeApp.Api.DTOs;
using BlazorPracticeApp.Api.Interfaces;
using BlazorPracticeApp.Api.Models;
using BlazorPracticeApp.ApiRequest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace BlazorPracticeApp.Api.Services
{
    public class MovieService : IMovieService
    {
        public readonly ContextDb context;

        public MovieService(ContextDb _context)
        {
            context = _context;
        }

        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await context.Movies.ToListAsync();
            return new OkObjectResult(new
            {
                status = true,
                list = movies
            });
        }


        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await context.Movies.FindAsync(id);
           
            if (movie == null)
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = $"Фильм с id {id} не существует"
                });
            }

            return new OkObjectResult(new
            {
                status = true,
                list = movie
            });
            
        }


        public async Task<IActionResult> CreateMovie(NewMovieDto newMovie)
        {
            var movieName = await context.Movies.FirstOrDefaultAsync(m => m.Name == newMovie.Name);

            if (movieName == null)
            {
                var movie = new Movie()
                {
                    Name = newMovie.Name,
                    Description = newMovie.Description,
                    Genre = newMovie.Genre,
                    ReleaseDate = newMovie.ReleaseDate,
                    Rating = newMovie.Rating,
                };

                await context.Movies.AddAsync(movie);
                await context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    status = true,
                    message = "Фильм успешно добавлен"
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = "Фильм с таким названием существует"
                });

            }
        }



        public async Task<IActionResult> UpdateMovie(int id, UpdateMovieDto updateMovie)
        {

            var movie = await context.Movies.FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = $"Фильм с id {id} не существует"
                });
            }

            var existingMovie = await context.Movies.FirstOrDefaultAsync(p => p.Name == updateMovie.Name && p.Id != id);

            if (existingMovie != null)
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = "Такой фильм уже существует"
                });
            }

            movie.Name = updateMovie.Name;
            movie.Description = updateMovie.Description;
            movie.Genre = updateMovie.Genre;
            movie.ReleaseDate = updateMovie.ReleaseDate;
            movie.Rating = updateMovie.Rating;

            await context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true,
                message = "Фильм успешно обновлен"
            });
        }

        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = context.Movies.FirstOrDefault(m => m.Id == id);

            if (movie != null)
            {
                context.Movies.Remove(movie);
                await context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    status = true,
                    message = "Фильм успешнло удален"
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    status = false,
                    message = $"Фильм с id {id} не существует"
                }
                );
            }

        }




    }
}

