using BlazorPracticeApp.ApiRequest.Models;
using System.Net.Http.Json;
using System.Text.Json;


namespace BlazorPracticeApp.ApiRequest
{
    public class ApiRequest
    {
        private readonly HttpClient httpClient;
        private string? token;
            
        public ApiRequest(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public void SetToken(string t)
        {
            token = t;
        }   
        public async Task<GetAllUsers> GetAllUsers()
        {
            var url = "/api/GetAll";

            if (string.IsNullOrEmpty(token))
                throw new Exception("Нет токена");

            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new GetAllUsers
                {
                    status = false,
                    list = new List<User>()
                };
            }

            var deserializeResult = JsonSerializer.Deserialize<GetAllUsers>(
                result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return deserializeResult ?? new GetAllUsers { status = false, list = new List<User>() };
        }

        public async Task<ResultActionUser> CreateUser(User createUser)
        {
            var url = "/api/CreateUser";

            if (string.IsNullOrEmpty(token))
                throw new Exception("Нет токена");

            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = await httpClient.PostAsJsonAsync(url, createUser);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new ResultActionUser
                {
                    status = false,
                    message = $"Ошибка сервера: {(int)response.StatusCode} {response.StatusCode}"
                };
            }

            var trimmed = result.TrimStart();
            if (string.IsNullOrWhiteSpace(result) || trimmed[0] != '{')
            {
                return new ResultActionUser
                {
                    status = false,
                    message = "Некорректный ответ сервера"
                };
            }

            var deserializeResult = JsonSerializer.Deserialize<ResultActionUser>(
                result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return deserializeResult ?? new ResultActionUser { status = false, message = "Не удалось прочитать ответ сервера" };
        }

        public async Task<ResultActionUser> UpdateUser(int id, User updateUser)
        {
            var url = $"/api/UpdateUser/{id}";

            if (string.IsNullOrEmpty(token))
                throw new Exception("Нет токена");

            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = await httpClient.PutAsJsonAsync(url, updateUser);
            var result = await response.Content.ReadAsStringAsync();
            var deserializeResult = JsonSerializer.Deserialize<ResultActionUser>(result);

            return deserializeResult ?? new ResultActionUser();
        }

        public async Task<ResultActionUser> DeleteUser(int id)
        {
            var url = $"/api/DeleteUser/{id}";

            if (string.IsNullOrEmpty(token))
                throw new Exception("Нет токена");

            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = await httpClient.DeleteAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var deserializeResult = JsonSerializer.Deserialize<ResultActionUser>(result);

            return deserializeResult ?? new ResultActionUser();
        }

        public async Task<ResultAuth> AuthUser(RequestAuth requestAuth)
        {
            var url = "/api/AuthUser";

            var response = await httpClient.PostAsJsonAsync(url, requestAuth);
            var result = await response.Content.ReadAsStringAsync();
            var deserializeResult = JsonSerializer.Deserialize<ResultAuth>(result);
            return deserializeResult ?? new ResultAuth();
        }

        public async Task<ResultRegistration> RegistrationUser(CreateUser createUser)
        {
            var url = "/api/RegUser";

            var responce = await httpClient.PostAsJsonAsync(url, createUser);
            var result = await responce.Content.ReadAsStringAsync();
            var deserializeResult = JsonSerializer.Deserialize<ResultRegistration>(result);
            return deserializeResult ?? new ResultRegistration();
        }
    }
}
    