
using IntegrationTests.Factories;
using QuizDev.Core.DTOs.AnswerOptions;
using QuizDev.Core.DTOs.Questions;
using QuizDev.Core.DTOs.Quizzes;
using QuizDev.Core.Entities;

namespace IntegrationTests.Controllers;

public class MatchesControllerTests : IClassFixture<QuizDevWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly QuizDevWebApplicationFactory _factory;

    public MatchesControllerTests(QuizDevWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateMatch_ShouldReturnCreated()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser3", "test3211@gmail.com");
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
            QuizId = quizId,
            Options = new List<CreateAnswerOptionInQuestionDto>
            {
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = false, Response = "Sample Option 1" },
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = true, Response = "Sample Option 2" }
            }
        };
        await Utils.CreateQuestionAsync(_client, token, questionDto);

        await Utils.ToggleQuizAsync(_client, token, quizId);

        // Act
        var matchData = await Utils.CreateMatchAsync(_client, token, (Guid) quizData.id);

        // Assert
        Assert.NotNull(matchData.matchId);
        Assert.NotNull(matchData.nextQuestion);
    }

    [Fact]
    public async Task SubmitResponse_ShouldReturnCreated()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser2", "test123@gmail.com");
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
            QuizId = quizId,
            Options = new List<CreateAnswerOptionInQuestionDto>
            {
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = false, Response = "Sample Option 1" },
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = true, Response = "Sample Option 2" }
            }
        };
        await Utils.CreateQuestionAsync(_client, token, questionDto);
        await Utils.ToggleQuizAsync(_client, token, quizId);
        var matchData = await Utils.CreateMatchAsync(_client, token, quizId);

        var optionId = (Guid) matchData.nextQuestion.options[0].id;

        // Act
        var responseData = await Utils.SubmitResponseAsync(_client, token, (Guid) matchData.matchId, optionId);

        // Assert
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task GetNextQuestion_ShouldReturnOk()
    {
        // Arrange
        var userData = await Utils.CreateUserAsync(_client, "testUser23", "test123123@gmail.com");
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
            QuizId = quizId,
            Options = new List<CreateAnswerOptionInQuestionDto>
            {
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = false, Response = "Sample Option 1" },
                new CreateAnswerOptionInQuestionDto { IsCorrectOption = true, Response = "Sample Option 2" }
            }
        };
        await Utils.CreateQuestionAsync(_client, token, questionDto);
        await Utils.ToggleQuizAsync(_client, token, quizId);
        var matchData = await Utils.CreateMatchAsync(_client, token, quizId);

        // Act
        var nextQuestionData = await Utils.GetNextQuestionAsync(_client, token, (Guid) matchData.matchId);

        // Assert
        Assert.NotNull(nextQuestionData);
        Assert.NotNull(nextQuestionData.question);
        Assert.True((bool)nextQuestionData.isLastQuestion);
    }
}

