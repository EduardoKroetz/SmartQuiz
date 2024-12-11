using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.DTOs.Reviews;
using SmartQuiz.Application.DTOs.Users;

namespace IntegrationTests;

public static class Utils
{
    public static async Task<dynamic> CreateUserAsync(HttpClient client, string username, string email, string password = "password")
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

    public static async Task<dynamic> CreateQuizAsync(HttpClient client, string token, EditorQuizDto dto)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsJsonAsync("api/quizzes", dto);
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> GetQuizAsync(HttpClient client, string token, Guid quizId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync($"api/quizzes/{quizId}");
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

    public static async Task<dynamic> ToggleQuizAsync(HttpClient client, string token, Guid quizId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsJsonAsync($"api/quizzes/toggle/{quizId}", new { });
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> UpdateQuizAsync(HttpClient client, string token, EditorQuizDto dto, Guid quizId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PutAsJsonAsync($"api/quizzes/{quizId}", dto);
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> DeleteQuizAsync(HttpClient client, string token, Guid quizId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.DeleteAsync($"api/quizzes/{quizId}");
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> GetQuestionsByQuizAsync(HttpClient client, string token, Guid quizId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync($"api/quizzes/{quizId}/questions");
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> CreateMatchAsync(HttpClient client, string token, Guid quizId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsJsonAsync($"api/matches/play/quiz/{quizId}", new { });
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> SubmitResponseAsync(HttpClient client, string token, Guid matchId, Guid optionId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsJsonAsync($"api/matches/{matchId}/submit/{optionId}", new { });
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> GetNextQuestionAsync(HttpClient client, string token, Guid matchId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync($"api/matches/{matchId}/next-question");
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> EndMatchAsync(HttpClient client, string token, Guid matchId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsJsonAsync($"/api/matches/{matchId}/end", new { });
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> GetMatchResultAsync(HttpClient client, string token, Guid matchId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync($"/api/matches/{matchId}/responses");
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> ListMatchesAsync(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync($"/api/matches?status=created&reviewed=false&orderby=score&pageSize=10&pageNumber=1");
        //response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> DeleteMatchAsync(HttpClient client, string token, Guid matchId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.DeleteAsync($"/api/matches/{matchId}");
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }


    public static async Task<dynamic> CreateAnswerOption(HttpClient client, string token, CreateAnswerOptionDto dto)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsJsonAsync($"/api/answeroptions", dto);
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> DeleteAnswerOption(HttpClient client, string token, Guid answerOptionId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.DeleteAsync($"/api/answeroptions/{answerOptionId}");
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> CreateReviewAsync(HttpClient client, string token, CreateReviewDto dto)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsJsonAsync($"/api/reviews", dto);
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> DeleteReviewAsync(HttpClient client, string token, Guid reviewId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.DeleteAsync($"/api/reviews/{reviewId}");
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> UpdateReviewAsync(HttpClient client, string token, UpdateReviewDto dto, Guid reviewId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PutAsJsonAsync($"/api/reviews/{reviewId}", dto);
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    public static async Task<dynamic> GetReviewDetailsAsync(HttpClient client, string token, Guid reviewId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync($"/api/reviews/{reviewId}");
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        return content.data;
    }

    // SEEDs
    public static async Task<Guid> SeedMatchAsync(HttpClient client, string token)
    {
        var quizId = await SeedQuizAsync(client, token);
        var matchData = await CreateMatchAsync(client, token, quizId);
        return (Guid)matchData.matchId;
    }

    public static async Task<Guid> SeedQuizAsync(HttpClient client, string token)
    {
        var quizDto = new EditorQuizDto
        {
            Title = "Sample Quiz",
            Description = "Sample Description Sample Description Sample Description Sample Description Sample Description",
            Expires = false,
            Difficulty = "medium",
            Theme = "Sample Quiz"
        };

        var quizData = await CreateQuizAsync(client, token, quizDto);
        var quizId = (Guid)quizData.id;
        var questionDto = new CreateQuestionDto
        {
            Text = "Sample Question",
            QuizId = quizId,
            Order = 0,
            Options = new List<CreateAnswerOptionInQuestionDto>
            {
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = false, Response = "Sample Option 1" },
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = true, Response = "Sample Option 2" },
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = false, Response = "Sample Option 3" },
            }
        };
        await CreateQuestionAsync(client, token, questionDto);
        await ToggleQuizAsync(client, token, quizId);
        return quizId;
    }

    public static async Task<string> SeedUserAsync(HttpClient client)
    {
        var random = new Random().Next(0, 10000);
        var userData = await CreateUserAsync(client, "testUser23", $"test{random}@gmail.com");
        return (string)userData.token;
    }

    public static async Task<Guid> SeedReviewAsync(HttpClient client, string token)
    {
        var matchId = await SeedMatchAsync(client, token);
        await EndMatchAsync(client, token, matchId);
        var reviewDto = new CreateReviewDto { MatchId = matchId, Description = "Ok", Rating = 6 };
        var response = await CreateReviewAsync(client, token, reviewDto);
        return (Guid)response.id;
    }

    public static async Task<Guid> SeedQuestionAsync(HttpClient client, string token, Guid quizId)
    {
        var questionDto = new CreateQuestionDto
        {
            Text = "Sample Question",
            QuizId = quizId,
            Options = new List<CreateAnswerOptionInQuestionDto>
            {
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = false, Response = "Sample Option 1" },
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = true, Response = "Sample Option 2" }
            }
        };
        var questionResponse = await Utils.CreateQuestionAsync(client, token, questionDto);
        return (Guid)questionResponse.id;
    }
}
