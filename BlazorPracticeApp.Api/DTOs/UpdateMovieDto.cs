namespace BlazorPracticeApp.Api.DTOs
{
    public class UpdateMovieDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public float Rating { get; set; }
    }
}
