namespace BlazorPracticeApp.ApiRequest.Models
{
    public class GenresListResult
    {
        public bool status { get; set; }
        public List<string> list { get; set; } = new();
        public string? message { get; set; }
    }

    public class GenreActionResult
    {
        public bool status { get; set; }
        public string? message { get; set; }
        public string? genre { get; set; }
    }
}

