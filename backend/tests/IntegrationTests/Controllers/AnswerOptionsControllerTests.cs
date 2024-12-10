
using IntegrationTests.Factories;
using SmartQuiz.Core.DTOs.AnswerOptions;

namespace IntegrationTests.Controllers;

public class AnswerOptionsControllerTests : IClassFixture<SmartQuizWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly SmartQuizWebApplicationFactory _factory;

    public AnswerOptionsControllerTests(SmartQuizWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateAnswerOption_ShouldReturnOk()
    {
        //Arrange
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);
        var questions = await Utils.GetQuestionsByQuizAsync(_client, token, quizId);
        var questionId = (Guid)questions[0].id;
        var dto = new CreateAnswerOptionDto { IsCorrectOption = false, QuestionId = questionId, Response = "Test" };

        //Act
        var response = await Utils.CreateAnswerOption(_client, token, dto);

        //Assert
        Assert.NotNull(response.answerOptionId);
        Assert.Equal(questionId, (Guid)response.questionId);
    }

    [Fact]
    public async Task DeleteAnswerOption_ShouldReturnOk()
    {
        //Arrange
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);
        var questions = await Utils.GetQuestionsByQuizAsync(_client, token, quizId);
        var questionId = (Guid)questions[0].id;
        var dto = new CreateAnswerOptionDto { IsCorrectOption = false, QuestionId = questionId, Response = "Test" };
        var createAnswerResponse = await Utils.CreateAnswerOption(_client, token, dto);
        var answerOptionId = (Guid)createAnswerResponse.answerOptionId;

        //Act
        var response = await Utils.DeleteAnswerOption(_client, token, answerOptionId);

        //Assert
        Assert.NotNull(response);
    }
}
