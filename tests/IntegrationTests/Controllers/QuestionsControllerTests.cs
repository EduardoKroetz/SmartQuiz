using IntegrationTests.Factories;
using Newtonsoft.Json;
using QuizDev.Core.DTOs.AnswerOptions;
using QuizDev.Core.DTOs.Questions;
using QuizDev.Core.DTOs.Quizzes;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTests.Controllers;

public class QuestionsControllerTests : IClassFixture<QuizDevWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly QuizDevWebApplicationFactory _factory;

    public QuestionsControllerTests(QuizDevWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateQuestion_ShouldReturnOk()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser", "test@gmail.com");
        var token = (string)userData.token;
        var quizData = await Utils.CreateQuizAsync(_client, token, new EditorQuizDto { Title = "test", Description = "test", Expires = false });

        var questionDto = new CreateQuestionDto
        {
            Text = "Sample Question",
            QuizId = (Guid) quizData.id,
            Options = new List<CreateAnswerOptionInQuestionDto>
            {
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = false, Response = "Sample Option 1" },
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = true, Response = "Sample Option 2" }
            }
        };

        // Act
        var content = await Utils.CreateQuestionAsync(_client, token, questionDto);

        // Assert
        Assert.NotNull(content.id);
    }

    [Fact]
    public async Task GetQuestionDetails_ShouldReturnOk()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser", "junior@gmail.com");
        var token = (string)userData.token;

        var quizData = await Utils.CreateQuizAsync(_client, token, new EditorQuizDto { Title = "test", Description = "test", Expires = false });

        var questionDto = new CreateQuestionDto
        {
            Text = "Sample Question",
            QuizId = (Guid)quizData.id,
            Options = new List<CreateAnswerOptionInQuestionDto>
            {
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = false, Response = "Sample Option 1" },
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = true, Response = "Sample Option 2" }
            }
        };
        var questionResponse = await Utils.CreateQuestionAsync(_client, token, questionDto);
        var questionId = (Guid)questionResponse.id;

        // Act
        var response = await _client.GetAsync($"api/questions/{questionId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        Assert.Equal("Sample Question", (string)content.data.text);
        Assert.Equal(questionDto.QuizId, (Guid)content.data.quizId);
        Assert.NotNull(content.data.id);

    }

    [Fact]
    public async Task UpdateQuestion_ShouldReturnOk()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser", "leonardo@gmail.com");
        var token = (string)userData.token;

        var quizData = await Utils.CreateQuizAsync(_client, token, new EditorQuizDto { Title = "test", Description = "test", Expires = false });

        var questionDto = new CreateQuestionDto
        {
            Text = "Initial Question",
            QuizId = (Guid)quizData.id,
            Options = new List<CreateAnswerOptionInQuestionDto>
        {
            new CreateAnswerOptionInQuestionDto { IsCorrectOption = false, Response = "Option 1" },
            new CreateAnswerOptionInQuestionDto { IsCorrectOption = true, Response = "Option 2" }
        }
        };
        var questionResponse = await Utils.CreateQuestionAsync(_client, token, questionDto);
        var questionId = (Guid)questionResponse.id;

        var updateDto = new UpdateQuestionDto { Text = "Updated Question Text" };
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.PatchAsJsonAsync($"api/questions/{questionId}/text", updateDto);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        Assert.Equal("Updated Question Text", (string)content.data.text);
        Assert.Equal(questionId, (Guid)content.data.id);
    }

    [Fact]
    public async Task DeleteQuestion_ShouldReturnOk()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser", "pedro@gmail.com");
        var token = (string)userData.token;

        var quizData = await Utils.CreateQuizAsync(_client, token, new EditorQuizDto { Title = "test", Description = "test", Expires = false });

        var questionDto = new CreateQuestionDto
        {
            Text = "Question to Delete",
            QuizId = (Guid)quizData.id,
            Options = new List<CreateAnswerOptionInQuestionDto>
        {
            new CreateAnswerOptionInQuestionDto { IsCorrectOption = false, Response = "Option 1" },
            new CreateAnswerOptionInQuestionDto { IsCorrectOption = true, Response = "Option 2" }
        }
        };
        var questionResponse = await Utils.CreateQuestionAsync(_client, token, questionDto);
        var questionId = (Guid)questionResponse.id;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.DeleteAsync($"api/questions/{questionId}");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateCorrectAnswerOption_ShouldReturnOk()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser", "lucas@gmail.com");
        var token = (string)userData.token;

        var quizData = await Utils.CreateQuizAsync(_client, token, new EditorQuizDto { Title = "test", Description = "test", Expires = false });

        var questionDto = new CreateQuestionDto
        {
            Text = "Question with Options",
            QuizId = (Guid)quizData.id,
            Options = new List<CreateAnswerOptionInQuestionDto>
        {
            new CreateAnswerOptionInQuestionDto { IsCorrectOption = false, Response = "Option 1" },
            new CreateAnswerOptionInQuestionDto { IsCorrectOption = true, Response = "Option 2" }
        }
        };
        var questionResponse = await Utils.CreateQuestionAsync(_client, token, questionDto);
        var questionId = (Guid)questionResponse.id;
        var question = await Utils.GetQuestionAsync(_client, questionId);
        var answerOptionId = question.options[0].id;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.PatchAsJsonAsync($"api/questions/{questionId}/correct-option/{answerOptionId}", new { });

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAnswerOptions_ShouldReturnOk()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser", "joao@gmail.com");
        var token = (string)userData.token;

        var quizData = await Utils.CreateQuizAsync(_client, token, new EditorQuizDto { Title = "test", Description = "test", Expires = false });

        var questionDto = new CreateQuestionDto
        {
            Text = "Question with Options",
            QuizId = (Guid)quizData.id,
            Options = new List<CreateAnswerOptionInQuestionDto>
        {
            new CreateAnswerOptionInQuestionDto { IsCorrectOption = false, Response = "Option 1" },
            new CreateAnswerOptionInQuestionDto { IsCorrectOption = true, Response = "Option 2" }
        }
        };
        var questionResponse = await Utils.CreateQuestionAsync(_client, token, questionDto);
        var questionId = (Guid)questionResponse.id;

        // Act
        var data = await Utils.GetQuestionAsync(_client, questionId);

        // Assert
        Assert.NotNull(data);
    }

}
