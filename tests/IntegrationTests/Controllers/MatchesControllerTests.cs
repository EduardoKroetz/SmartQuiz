
using IntegrationTests.Factories;

namespace IntegrationTests.Controllers;

public class MatchesControllerTests : IClassFixture<SmartQuizWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly SmartQuizWebApplicationFactory _factory;

    public MatchesControllerTests(SmartQuizWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateMatch_ShouldReturnCreated()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);

        // Act
        var matchData = await Utils.CreateMatchAsync(_client, token, quizId);

        // Assert
        Assert.NotNull(matchData.matchId);
        Assert.NotNull(matchData.nextQuestion);
    }

    [Fact]
    public async Task SubmitResponse_ShouldReturnCreated()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);
        var quizId = await Utils.SeedQuizAsync(_client, token);
        var matchData = await Utils.CreateMatchAsync(_client, token, quizId);
        var optionId = (Guid)matchData.nextQuestion.options[0].id;

        // Act
        var responseData = await Utils.SubmitResponseAsync(_client, token, (Guid)matchData.matchId, optionId);

        // Assert
        Assert.NotNull(responseData);
    }

    [Fact]
    public async Task GetNextQuestion_ShouldReturnOk()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);
        var matchId = await Utils.SeedMatchAsync(_client, token);

        // Act
        var nextQuestionData = await Utils.GetNextQuestionAsync(_client, token, matchId);

        // Assert
        Assert.NotNull(nextQuestionData);
        Assert.NotNull(nextQuestionData.question);
        Assert.True((bool)nextQuestionData.isLastQuestion);
    }

    [Fact]
    public async Task EndMatch_ShouldReturnOk()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);
        var matchId = await Utils.SeedMatchAsync(_client, token);

        // Act
        var endMatchData = await Utils.EndMatchAsync(_client, token, matchId);

        // Assert
        Assert.NotNull(endMatchData.id);
    }

    [Fact]
    public async Task GetMatchResult_ShouldReturnOk()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);
        var matchId = await Utils.SeedMatchAsync(_client, token);
        await Utils.EndMatchAsync(_client, token, matchId);

        // Act
        var matchResult = await Utils.GetMatchResultAsync(_client, token, matchId);

        // Assert
        Assert.NotNull(matchResult);
    }

    [Fact]
    public async Task ListMatches_ShouldReturnOk()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);

        // Act
        var matchesList = await Utils.ListMatchesAsync(_client, token);

        // Assert
        Assert.NotNull(matchesList);
    }

    [Fact]
    public async Task DeleteMatch_ShouldReturnOk()
    {
        // Arrange
        var token = await Utils.SeedUserAsync(_client);
        var matchId = await Utils.SeedMatchAsync(_client, token);

        // Act
        var response = await Utils.DeleteMatchAsync(_client, token, matchId);

        // Assert
        Assert.NotNull(response.id);
    }
}

