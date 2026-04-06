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

        

        public async Task<MoviesListResult> GetMovies()
        {
            var url = "/api/movies";

            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new MoviesListResult
                {
                    status = false,
                    list = new List<Movie>(),
                    message = $"Ошибка сервера: {(int)response.StatusCode} {response.StatusCode}"
                };
            }

            var deserializeResult = JsonSerializer.Deserialize<MoviesListResult>(
                result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return deserializeResult ?? new MoviesListResult { status = false, list = new List<Movie>() };
        }

        public async Task<GenresListResult> GetGenres()
        {
            var url = "/api/genres";

            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new GenresListResult
                {
                    status = false,
                    list = new List<string>(),
                    message = $"Ошибка сервера: {(int)response.StatusCode} {response.StatusCode}"
                };
            }

            var deserializeResult = JsonSerializer.Deserialize<GenresListResult>(
                result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return deserializeResult ?? new GenresListResult { status = false, list = new List<string>() };
        }

        public async Task<GenreActionResult> CreateGenre(string name)
        {
            var url = "/api/genres";

            if (string.IsNullOrEmpty(token))
                throw new Exception("Нет токена");

            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = await httpClient.PostAsJsonAsync(url, new { name });
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new GenreActionResult
                {
                    status = false,
                    message = $"Ошибка сервера: {(int)response.StatusCode} {response.StatusCode}"
                };
            }

            var deserializeResult = JsonSerializer.Deserialize<GenreActionResult>(
                result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return deserializeResult ?? new GenreActionResult { status = false, message = "Не удалось прочитать ответ сервера" };
        }

        public async Task<MovieItemResult> GetMovieById(int id)
        {
            var url = $"/api/movies/{id}";

            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new MovieItemResult
                {
                    status = false,
                    message = $"Ошибка сервера: {(int)response.StatusCode} {response.StatusCode}"
                };
            }

            var deserializeResult = JsonSerializer.Deserialize<MovieItemResult>(
                result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return deserializeResult ?? new MovieItemResult { status = false };
        }

        public async Task<MovieActionResult> CreateMovie(Movie movie)
        {
            var url = "/api/movies";

            if (string.IsNullOrEmpty(token))
                throw new Exception("Нет токена");

            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = await httpClient.PostAsJsonAsync(url, movie);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new MovieActionResult
                {
                    status = false,
                    message = $"Ошибка сервера: {(int)response.StatusCode} {response.StatusCode}"
                };
            }

            var trimmed = result.TrimStart();
            if (string.IsNullOrWhiteSpace(result) || trimmed[0] != '{')
            {
                return new MovieActionResult
                {
                    status = false,
                    message = "Некорректный ответ сервера"
                };
            }

            var deserializeResult = JsonSerializer.Deserialize<MovieActionResult>(
                result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return deserializeResult ?? new MovieActionResult { status = false, message = "Не удалось прочитать ответ сервера" };
        }

        public async Task<MovieActionResult> UpdateMovie(int id, Movie movie)
        {
            var url = $"/api/movies/{id}";

            if (string.IsNullOrEmpty(token))
                throw new Exception("Нет токена");

            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = await httpClient.PutAsJsonAsync(url, movie);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new MovieActionResult
                {
                    status = false,
                    message = $"Ошибка сервера: {(int)response.StatusCode} {response.StatusCode}"
                };
            }

            var trimmed = result.TrimStart();
            if (string.IsNullOrWhiteSpace(result) || trimmed[0] != '{')
            {
                return new MovieActionResult
                {
                    status = false,
                    message = "Некорректный ответ сервера"
                };
            }

            var deserializeResult = JsonSerializer.Deserialize<MovieActionResult>(
                result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return deserializeResult ?? new MovieActionResult { status = false, message = "Не удалось прочитать ответ сервера" };
        }

        public async Task<MovieActionResult> DeleteMovie(int id)
        {
            var url = $"/api/movies/{id}";

            if (string.IsNullOrEmpty(token))
                throw new Exception("Нет токена");

            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = await httpClient.DeleteAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new MovieActionResult
                {
                    status = false,
                    message = $"Ошибка сервера: {(int)response.StatusCode} {response.StatusCode}"
                };
            }

            var trimmed = result.TrimStart();
            if (string.IsNullOrWhiteSpace(result) || trimmed[0] != '{')
            {
                return new MovieActionResult
                {
                    status = false,
                    message = "Некорректный ответ сервера"
                };
            }

            var deserializeResult = JsonSerializer.Deserialize<MovieActionResult>(
                result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return deserializeResult ?? new MovieActionResult { status = false, message = "Не удалось прочитать ответ сервера" };
        }

        public async Task<List<ChatMessageDto>> GetMovieChatMessages(int movieId)
        {
            var response = await httpClient.GetAsync($"/api/Chat/movie/{movieId}");
            var json = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new List<ChatMessageDto>();

            return JsonSerializer.Deserialize<List<ChatMessageDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<ChatMessageDto>();
        }

        public async Task<List<ChatMessageDto>> GetPrivateChatMessages(int userId1, int userId2)
        {
            var response = await httpClient.GetAsync($"/api/Chat/private/{userId1}/{userId2}");
            var json = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return new List<ChatMessageDto>();

            return JsonSerializer.Deserialize<List<ChatMessageDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<ChatMessageDto>();
        }

        public async Task<ChatMessageDto?> SendChatMessage(int senderId, SendMessageDto dto)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/Chat/send/{senderId}", dto);
            if (!response.IsSuccessStatusCode)
                return null;
                     
            return await response.Content.ReadFromJsonAsync<ChatMessageDto>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> DeleteChatMessage(int messageId)
        {
            var response = await httpClient.DeleteAsync($"/api/Chat/{messageId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditChatMessage(int messageId, string newText)
        {
            var json = JsonSerializer.Serialize(newText);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"/api/Chat/{messageId}", content);
            return response.IsSuccessStatusCode;
        }

    }
}

    