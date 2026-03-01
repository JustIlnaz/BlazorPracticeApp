using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlazorPracticeApp.Api.Models
{
    public class Session
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User? Users { get; set; }
    }
}
