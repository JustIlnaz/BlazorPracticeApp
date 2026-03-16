using System.ComponentModel.DataAnnotations;

namespace BlazorPracticeApp.Api.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string? Name {  get; set; }
        public string? Description  { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public float Rating { get; set; }
        public string? ImageUrl { get; set; }
    }
}   
