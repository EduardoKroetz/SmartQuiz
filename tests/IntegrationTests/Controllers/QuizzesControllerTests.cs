using IntegrationTests.Factories;
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
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);

        // Act
        var data = await Utils.GetQuizAsync(_client, token, quizId);

        // Assert
        Assert.NotNull(data.title);
        Assert.NotNull(data.id);
        Assert.NotNull(data.description);
        Assert.NotNull(data.expires);
        Assert.NotNull(data.expiresInSeconds);
        Assert.NotNull(data.isActive);
        Assert.NotNull(data.userId);
    }

    [Fact]
    public async Task ToggleQuiz_ShouldToggleActivation()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);

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
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);

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
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);

        // Act
        var deleteResponse = await Utils.DeleteQuizAsync(_client, token, quizId);

        // Assert
        Assert.Equal(quizId, (Guid)deleteResponse.id);
    }

    [Fact]
    public async Task GetQuestionsByQuiz_ShouldReturnQuestions()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);

        // Act
        var questions = await Utils.GetQuestionsByQuizAsync(_client, token, quizId);

        // Assert
        Assert.NotNull(questions);
    }
}
