using BlazorPracticeApp.Api.ContextDatabase;
using BlazorPracticeApp.Api.DTOs;
using BlazorPracticeApp.Api.Interfaces;
using BlazorPracticeApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPracticeApp.Api.Services
{
    public class MovieService : IMovieService
    {
        public readonly ContextDb context;

        public MovieService(ContextDb _context)
        {
            context = _context;
        }

        private static DateTime ToUtc(DateTime value)
        {
            if (value.Kind == DateTimeKind.Utc)
            {
                return value;

            }

            if (value.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
            return value.ToUniversalTime();
        }

        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await context.Movies
                .Include(m => m.Genre)
                .ToListAsync();

            var list = movies.Select(m => new
            {
                m.Id,
                m.Name,
                m.Description,
                Genre = m.Genre.NameGenre,
                m.ReleaseDate,
                m.Rating,
                m.ImageUrl
            }).ToList();

            return new OkObjectResult(new
            {
                status = true,
                list
            });
        }


        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
           
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
                list = new
                {
                    movie.Id,
                    movie.Name,
                    movie.Description,
                    Genre = movie.Genre.NameGenre,
                    movie.ReleaseDate,
                    movie.Rating,
                    movie.ImageUrl
                }
            });
            
        }


        public async Task<IActionResult> CreateMovie(NewMovieDto newMovie)
        {
            var movieName = await context.Movies.FirstOrDefaultAsync(m => m.Name == newMovie.Name);

            if (movieName == null)
            {
                if (string.IsNullOrWhiteSpace(newMovie.Genre))
                {
                    return new OkObjectResult(new
                    {
                        status = false,
                        message = "Жанр обязателен"
                    });
                }

                var genre = await context.Genres.FirstOrDefaultAsync(g => g.NameGenre == newMovie.Genre);
                if (genre == null)
                {
                    return new OkObjectResult(new
                    {
                        status = false,
                        message = "Такого жанра не существует. Сначала добавьте жанр."
                    });
                }

                var movie = new Movie()
                {
                    Name = newMovie.Name,
                    Description = newMovie.Description,
                    GenreId = genre.Id,
                    ReleaseDate = ToUtc(newMovie.ReleaseDate),
                    Rating = newMovie.Rating,
                    ImageUrl = newMovie.ImageUrl
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

            if (!string.IsNullOrWhiteSpace(updateMovie.Genre))
            {
                var genre = await context.Genres.FirstOrDefaultAsync(g => g.NameGenre == updateMovie.Genre);
                if (genre == null)
                {
                    return new OkObjectResult(new
                    {
                        status = false,
                        message = "Такого жанра не существует. Сначала добавьте жанр."
                    });
                }

                movie.GenreId = genre.Id;
            }

            movie.Name = updateMovie.Name;
            movie.Description = updateMovie.Description;
            movie.ReleaseDate = ToUtc(updateMovie.ReleaseDate);
            movie.Rating = updateMovie.Rating;
            movie.ImageUrl = updateMovie.ImageUrl;

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

