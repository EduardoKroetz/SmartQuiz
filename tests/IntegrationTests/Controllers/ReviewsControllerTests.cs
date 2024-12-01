
using IntegrationTests.Factories;
using SmartQuiz.Core.DTOs.Reviews;

namespace IntegrationTests.Controllers;

public class ReviewsControllerTests : IClassFixture<SmartQuizWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly SmartQuizWebApplicationFactory _factory;

    public ReviewsControllerTests(SmartQuizWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateReview_ShouldReturnOk()
    {
        //Arrange
        var token = await Utils.SeedUserAsync(_client);
        var matchId = await Utils.SeedMatchAsync(_client, token);
        await Utils.EndMatchAsync(_client, token, matchId);
        var reviewDto = new CreateReviewDto { MatchId = matchId, Description = "Very cool!", Rating = 8 };

        //Act
        var response = await Utils.CreateReviewAsync(_client, token, reviewDto);

        //Assert
        Assert.NotNull(response.id);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturnOk()
    {
        //Arrange
        var token = await Utils.SeedUserAsync(_client);
        var reviewId = await Utils.SeedReviewAsync(_client, token);

        //Act
        var response = await Utils.DeleteReviewAsync(_client, token, reviewId);

        //Assert
        Assert.NotNull(response.id);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturnOk()
    {
        //Arrange
        var token = await Utils.SeedUserAsync(_client);
        var reviewId = await Utils.SeedReviewAsync(_client, token);
        var updateDto = new UpdateReviewDto { Description = "Cool!!", Rating = 8 };

        //Act
        var response = await Utils.UpdateReviewAsync(_client, token, updateDto, reviewId);

        //Assert
        Assert.NotNull(response.id);
    }

    [Fact]
    public async Task GetReviewDetails_ShouldReturnOk()
    {
        //Arrange
        var token = await Utils.SeedUserAsync(_client);
        var reviewId = await Utils.SeedReviewAsync(_client, token);

        //Act
        var response = await Utils.GetReviewDetailsAsync(_client, token, reviewId);

        //Assert
        Assert.NotNull(response.id);
        Assert.NotNull(response.description);
        Assert.NotNull(response.rating);
        Assert.NotNull(response.quizId);
        Assert.NotNull(response.matchId);
        Assert.NotNull(response.userId);
    }
}
