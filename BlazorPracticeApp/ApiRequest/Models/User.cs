namespace BlazorPracticeApp.ApiRequest.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? RoleId { get; set; }
    }

    public class GetAllUsers
    {
        public bool status { get; set; }
        public List<User> list { get; set; }
    }

    public class CreateUser
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? RoleId { get; set; }
    }

    public class ResultActionUser
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class ResultAuth
    {
        public bool status { get; set; }
        public string token { get; set; }
        public string message { get; set; }
        public int roleId { get; set; }
        public string? name { get; set; }
    }

    public class RequestAuth
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class ResultRegistration
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}
