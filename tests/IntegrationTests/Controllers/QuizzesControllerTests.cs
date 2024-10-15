using IntegrationTests.Factories;
using Newtonsoft.Json;
using QuizDev.Core.DTOs.AnswerOptions;
using QuizDev.Core.DTOs.Questions;
using QuizDev.Core.DTOs.Quizzes;

namespace IntegrationTests.Controllers;

public class QuizzesControllerTests : IClassFixture<QuizDevWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly QuizDevWebApplicationFactory _factory;

    public QuizzesControllerTests(QuizDevWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateQuiz_ShouldReturnCreated()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser", "test321@gmail.com");
        var token = (string)userData.token;

        var quizDto = new EditorQuizDto
        {
            Title = "Sample Quiz",
            Description = "Sample Description",
            Expires = false
        };

        // Act
        var quizData = await Utils.CreateQuizAsync(_client, token, quizDto);

        // Assert
        Assert.NotNull(quizData.id);
    }

    [Fact]
    public async Task GetQuizById_ShouldReturnQuizDetails()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser", "lucal@gmail.com");
        var token = (string)userData.token;

        var quizDto = new EditorQuizDto
        {
            Title = "Sample Quiz",
            Description = "Sample Description",
            Expires = false
        };
        var quizData = await Utils.CreateQuizAsync(_client, token, quizDto);
        var quizId = (Guid)quizData.id;

        // Act
        var response = await _client.GetAsync($"api/quizzes/{quizId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        Assert.Equal(quizDto.Title, (string)content.data.title);
    }

    [Fact]
    public async Task ToggleQuiz_ShouldToggleActivation()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser", "vitor@gmail.com");
        var token = (string)userData.token;

        var quizDto = new EditorQuizDto
        {
            Title = "Sample Quiz",
            Description = "Sample Description",
            Expires = false
        };
        var quizData = await Utils.CreateQuizAsync(_client, token, quizDto);
        var quizId = (Guid)quizData.id;

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
        await Utils.CreateQuestionAsync(_client, token, questionDto);

        // Act
        var toggleResponse = await Utils.ToggleQuizAsync(_client, token, quizId);

        // Assert
        Assert.Equal(quizId, (Guid)toggleResponse.quizId);
        Assert.True((bool)toggleResponse.isActive);
    }

    [Fact]
    public async Task UpdateQuiz_ShouldReturnUpdatedQuiz()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser", "test1@gmail.com");
        var token = (string)userData.token;

        var quizDto = new EditorQuizDto
        {
            Title = "Sample Quiz",
            Description = "Sample Description",
            Expires = false
        };
        var quizData = await Utils.CreateQuizAsync(_client, token, quizDto);
        var quizId = (Guid)quizData.id;

        var updatedQuizDto = new EditorQuizDto
        {
            Title = "Updated Quiz Title",
            Description = "Updated Description",
            Expires = false
        };

        // Act
        var updatedQuiz = await Utils.UpdateQuizAsync(_client, token, updatedQuizDto, quizId);

        // Assert
        Assert.Equal(quizId, (Guid)updatedQuiz.id);
    }

    [Fact]
    public async Task DeleteQuiz_ShouldReturnOk()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser", "test2@gmail.com");
        var token = (string)userData.token;

        var quizDto = new EditorQuizDto
        {
            Title = "Sample Quiz",
            Description = "Sample Description",
            Expires = false
        };
        var quizData = await Utils.CreateQuizAsync(_client, token, quizDto);
        var quizId = (Guid)quizData.id;

        // Act
        var deleteResponse = await Utils.DeleteQuizAsync(_client, token, quizId);

        // Assert
        Assert.Equal(quizId, (Guid)deleteResponse.id);
    }

    [Fact]
    public async Task GetQuestionsByQuiz_ShouldReturnQuestions()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser", "test3@gmail.com");
        var token = (string)userData.token;

        var quizDto = new EditorQuizDto
        {
            Title = "Sample Quiz",
            Description = "Sample Description",
            Expires = false
        };
        var quizData = await Utils.CreateQuizAsync(_client, token, quizDto);
        var quizId = (Guid)quizData.id;

        // Act
        var questions = await Utils.GetQuestionsByQuizAsync(_client, token, quizId);

        // Assert
        Assert.NotNull(questions);
    }


}
