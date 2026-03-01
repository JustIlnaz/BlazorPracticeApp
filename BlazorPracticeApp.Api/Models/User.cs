using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlazorPracticeApp.Api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        [ForeignKey("Roles")]
        public int? RoleId { get; set; }
        [JsonIgnore]
        public virtual Role Roles { get; set; }
        public virtual ICollection<Session>? Sessions { get; set; }
    }
}
