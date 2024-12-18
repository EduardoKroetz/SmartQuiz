using Moq;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.UseCases.Reviews;
using SmartQuiz.Application.DTOs.Reviews;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using Match = SmartQuiz.Core.Entities.Match;

namespace UnitTests.UseCases.Reviews;

public class CreateReviewUseCaseTests
{
    private readonly Mock<IReviewService> _reviewServiceMock;
    private readonly Mock<IMatchService> _matchServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly CreateReviewUseCase _useCase;

    public CreateReviewUseCaseTests()
    {
        _reviewServiceMock = new Mock<IReviewService>();
        _matchServiceMock = new Mock<IMatchService>();
        _authServiceMock = new Mock<IAuthService>();
        _useCase = new CreateReviewUseCase(_reviewServiceMock.Object, _matchServiceMock.Object, _authServiceMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenMatchDoesNotExist()
    {
        // Arrange
        var dto = new CreateReviewDto { MatchId = Guid.NewGuid() };
        _matchServiceMock.Setup(x => x.GetByIdAsync(dto.MatchId)).ReturnsAsync((Match)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _useCase.Execute(dto, Guid.NewGuid()));
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidOperationException_WhenMatchIsNotFinished()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var dto = new CreateReviewDto { MatchId = matchId };
        var match = new Match { Id = matchId, UserId = userId, Status = EMatchStatus.Created };

        _matchServiceMock.Setup(x => x.GetByIdAsync(matchId)).ReturnsAsync(match);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.Execute(dto, userId));
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidOperationException_WhenMatchIsAlreadyReviewed()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var dto = new CreateReviewDto { MatchId = matchId };
        var match = new Match { Id = matchId, UserId = userId, Status = EMatchStatus.Finished, Reviewed = true };

        _matchServiceMock.Setup(x => x.GetByIdAsync(matchId)).ReturnsAsync(match);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.Execute(dto, userId));
    }

    [Fact]
    public async Task Execute_ShouldThrowUnauthorizedAccessException_WhenUserIsNotMatchOwner()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var dto = new CreateReviewDto { MatchId = matchId };
        var match = new Match { Id = matchId, UserId = Guid.NewGuid(), Status = EMatchStatus.Finished }; // Outro usuário é o dono

        _authServiceMock.Setup(x => x.ValidateSameUser(match.UserId, userId))
            .Throws<UnauthorizedAccessException>();
        _matchServiceMock.Setup(x => x.GetByIdAsync(matchId)).ReturnsAsync(match);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _useCase.Execute(dto, userId));
    }

    [Fact]
    public async Task Execute_ShouldCreateReview_WhenValidConditions()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var dto = new CreateReviewDto { MatchId = matchId, Description = "Great match", Rating = 5 };
        var review = new Review { MatchId = matchId, UserId = userId };
        var match = new Match { Id = matchId, UserId = userId, Status = EMatchStatus.Finished, Reviewed = false, Quiz = new Quiz { UserId = userId}};

        _reviewServiceMock.Setup(x => x.CreateReview(It.IsAny<CreateReviewDto>(), It.IsAny<Match>(), It.IsAny<Guid>())).Returns(review);
        _matchServiceMock.Setup(x => x.GetByIdAsync(matchId)).ReturnsAsync(match);

        // Act
        await _useCase.Execute(dto, userId);

        // Assert
        _reviewServiceMock.Verify(x => x.AddAsync(It.IsAny<Review>()), Times.Once);
        _matchServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Match>()), Times.Once);
        _authServiceMock.Verify(x => x.ValidateSameUser(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
    }
}