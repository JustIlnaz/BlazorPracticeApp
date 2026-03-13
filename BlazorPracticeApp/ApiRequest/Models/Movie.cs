namespace BlazorPracticeApp.ApiRequest.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public float Rating { get; set; }
    }

    public class MoviesListResult
    {
        public bool status { get; set; }
        public List<Movie> list { get; set; } = new();
        public string? message { get; set; }
    }

    public class MovieItemResult
    {
        public bool status { get; set; }
        public Movie? list { get; set; }
        public string? message { get; set; }
    }

    public class MovieActionResult
    {
        public bool status { get; set; }
        public string? message { get; set; }
    }
}

