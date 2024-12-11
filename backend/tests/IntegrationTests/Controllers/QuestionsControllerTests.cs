using System.Net.Http.Json;
using IntegrationTests.Factories;
using Newtonsoft.Json;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.DTOs.Questions;

namespace IntegrationTests.Controllers;

public class QuestionsControllerTests : IClassFixture<SmartQuizWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly SmartQuizWebApplicationFactory _factory;

    public QuestionsControllerTests(SmartQuizWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateQuestion_ShouldReturnOk()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);

        var questionDto = new CreateQuestionDto
        {
            Text = "Sample Question 2",
            QuizId = quizId,
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
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);
        var questionId = await Utils.SeedQuestionAsync(_client, token, quizId);

        // Act
        var response = await Utils.GetQuestionAsync(_client, questionId);

        // Assert
        Assert.Equal("Sample Question", (string)response.text);
        Assert.Equal(quizId, (Guid)response.quizId);
        Assert.NotNull(response.id);

    }

    [Fact]
    public async Task UpdateQuestion_ShouldReturnOk()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);
        var questionId = await Utils.SeedQuestionAsync(_client, token, quizId);
        var updateDto = new UpdateQuestionDto { Text = "Updated Question Text" };

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
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);
        var questionId = await Utils.SeedQuestionAsync(_client, token, quizId);

        // Act
        var response = await _client.DeleteAsync($"api/questions/{questionId}");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateCorrectAnswerOption_ShouldReturnOk()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);
        var questionId = await Utils.SeedQuestionAsync(_client, token, quizId);
        var question = await Utils.GetQuestionAsync(_client, questionId);
        var answerOptionId = question.options[0].id;

        // Act
        var response = await _client.PatchAsJsonAsync($"api/questions/{questionId}/correct-option/{answerOptionId}", new { });

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAnswerOptions_ShouldReturnOk()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);
        var questionId = await Utils.SeedQuestionAsync(_client, token, quizId);

        // Act
        var data = await Utils.GetQuestionAsync(_client, questionId);

        // Assert
        Assert.NotNull(data);
    }

}
