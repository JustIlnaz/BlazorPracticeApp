namespace BlazorPracticeApp.Api.DTOs
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public string? ImageBase64 { get; set; }
        public DateTime SentAt { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public int? MovieId { get; set; }
        public int? RecipientUserId { get; set; }
    }

    public class SendMessageDto
    {
        public string Text { get; set; } = "";
        public string? ImageBase64 { get; set; }
        public int? MovieId { get; set; }
        public int? RecipientUserId { get; set; }
    }
}
