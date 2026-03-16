using System.ComponentModel.DataAnnotations;

namespace BlazorPracticeApp.Api.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; } 

        public string NameGenre { get; set; }
    }
}
