using System.ComponentModel.DataAnnotations;

namespace BlazorPracticeApp.Api.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string? Name {  get; set; }
        public string? Description  { get; set; }
        public string? Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public float Rating { get; set; }
    }
}   
