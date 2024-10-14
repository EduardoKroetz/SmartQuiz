using Newtonsoft.Json;
using QuizDev.Core.DTOs.AnswerOptions;
using QuizDev.Core.DTOs.Questions;
using QuizDev.Core.DTOs.Quizzes;
using QuizDev.Core.DTOs.Users;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTests;

public static class Utils
{
    public static async Task<dynamic> CreateUserAsync(HttpClient client ,string username, string email, string password = "password")
    {
        var createUserDto = new CreateUserDto { Username = username, Email = email, Password = password };
        var createUserResponse = await client.PostAsJsonAsync("api/accounts/register", createUserDto); //Criar novo usuário
        var userContent = JsonConvert.DeserializeObject<dynamic>(await createUserResponse.Content.ReadAsStringAsync());
        return userContent.data;
    }

    public static async Task<dynamic> CreateQuestionAsync(HttpClient client, string token, CreateQuestionDto questionDto)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsJsonAsync("api/questions", questionDto);
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> CreateAnswerOptionAsync(HttpClient client, string token, CreateAnswerOptionDto dto)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsJsonAsync("api/answeroptions", dto);
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> CreateQuizAsync(HttpClient client, string token ,EditorQuizDto dto)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsJsonAsync("api/quizzes", dto);
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> GetQuestionAsync(HttpClient client, Guid questionId)
    {
        var response = await client.GetAsync($"api/questions/{questionId}");
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }
}
