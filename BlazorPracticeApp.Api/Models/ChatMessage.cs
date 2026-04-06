namespace BlazorPracticeApp.Api.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public string? ImageBase64 { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; }

        public int? MovieId { get; set; }      
        public Movie? Movie { get; set; }

        public int? RecipientUserId { get; set; } 
        public User? RecipientUser { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
