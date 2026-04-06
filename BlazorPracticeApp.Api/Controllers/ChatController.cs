using BlazorPracticeApp.Api.ContextDatabase;
using BlazorPracticeApp.Api.DTOs;
using BlazorPracticeApp.Api.Hubs;
using BlazorPracticeApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlazorPracticeApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ContextDb _db;
        private readonly IHubContext<ChatHub> _hub;

        public ChatController(ContextDb db, IHubContext<ChatHub> hub)
        {
            _db = db;
            _hub = hub;
        }

        [HttpGet("movie/{movieId}")]
        public async Task<List<ChatMessageDto>> GetMovieMessages(int movieId)
        {
            return await _db.ChatMessages
                .Where(m => m.MovieId == movieId && m.RecipientUserId == null && !m.IsDeleted)
                .OrderBy(m => m.SentAt)
                .Select(m => new ChatMessageDto
                {
                    Id = m.Id,
                    Text = m.Text,
                    ImageBase64 = m.ImageBase64,
                    SentAt = m.SentAt,
                    UserId = m.UserId,
                    UserName = m.User.Name ?? "Unknown",
                    MovieId = m.MovieId
                })
                .ToListAsync();
        }

        [HttpGet("private/{userId1}/{userId2}")]
        public async Task<List<ChatMessageDto>> GetPrivateMessages(int userId1, int userId2)
        {
            return await _db.ChatMessages
                .Where(m =>
                    m.RecipientUserId != null && !m.IsDeleted &&
                    ((m.UserId == userId1 && m.RecipientUserId == userId2) ||
                     (m.UserId == userId2 && m.RecipientUserId == userId1)))
                .OrderBy(m => m.SentAt)
                .Select(m => new ChatMessageDto
                {
                    Id = m.Id,
                    Text = m.Text,
                    ImageBase64 = m.ImageBase64,
                    SentAt = m.SentAt,
                    UserId = m.UserId,
                    UserName = m.User.Name ?? "Unknown",
                    RecipientUserId = m.RecipientUserId
                })
                .ToListAsync();
        }

        [HttpPost("send/{senderId}")]
        public async Task<IActionResult> SendMessage(int senderId, [FromBody] SendMessageDto dto)
        {
            var msg = new ChatMessage
            {
                Text = dto.Text,
                ImageBase64 = dto.ImageBase64,
                UserId = senderId,
                MovieId = dto.MovieId,
                RecipientUserId = dto.RecipientUserId,
                SentAt = DateTime.UtcNow
            };

            _db.ChatMessages.Add(msg);
            await _db.SaveChangesAsync();

            var user = await _db.Users.FindAsync(senderId);

            var result = new ChatMessageDto
            {
                Id = msg.Id,
                Text = msg.Text,
                ImageBase64 = msg.ImageBase64,
                SentAt = msg.SentAt,
                UserId = senderId,
                UserName = user?.Name ?? "Unknown",
                MovieId = msg.MovieId,
                RecipientUserId = msg.RecipientUserId
            };

            if (msg.MovieId.HasValue && msg.RecipientUserId == null)
            {
                await _hub.Clients.Group($"movie_{msg.MovieId}")
                    .SendAsync("ReceiveMessage", result);
            }
            else if (msg.RecipientUserId.HasValue)
            {
                string group = ChatHub.GetPrivateGroupName(senderId, msg.RecipientUserId.Value);
                await _hub.Clients.Group(group).SendAsync("ReceiveMessage", result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var msg = await _db.ChatMessages.FindAsync(id);
            if (msg == null) return NotFound();
            msg.IsDeleted = true;
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditMessage(int id, [FromBody] string newText)
        {
            var msg = await _db.ChatMessages.FindAsync(id);
            if (msg == null) return NotFound();
            msg.Text = newText;
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
