using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.UseCases.Reviews;
using SmartQuiz.Application.DTOs.Reviews;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Reviews;

public class CreateReviewUseCaseTests
{
    private readonly Mock<IReviewRepository> _mockReviewRepository;
    private readonly Mock<IMatchRepository> _mockMatchRepository;
    private readonly CreateReviewUseCase _useCase;

    public CreateReviewUseCaseTests()
    {
        _mockReviewRepository = new Mock<IReviewRepository>();
        _mockMatchRepository = new Mock<IMatchRepository>();
        _useCase = new CreateReviewUseCase(_mockReviewRepository.Object, _mockMatchRepository.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenMatchDoesNotExist()
    {
        // Arrange
        var dto = new CreateReviewDto { MatchId = Guid.NewGuid() };
        _mockMatchRepository.Setup(repo => repo.GetByIdAsync(dto.MatchId)).ReturnsAsync((SmartQuiz.Core.Entities.Match)null);

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
        var match = new SmartQuiz.Core.Entities.Match { Id = matchId, UserId = userId, Status = EMatchStatus.Created };

        _mockMatchRepository.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(match);

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
        var match = new SmartQuiz.Core.Entities.Match { Id = matchId, UserId = userId, Status = EMatchStatus.Finished, Reviewed = true };

        _mockMatchRepository.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(match);

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
        var match = new SmartQuiz.Core.Entities.Match { Id = matchId, UserId = Guid.NewGuid(), Status = EMatchStatus.Finished }; // Outro usuário é o dono

        _mockMatchRepository.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(match);

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
        var match = new SmartQuiz.Core.Entities.Match { Id = matchId, UserId = userId, Status = EMatchStatus.Finished, Reviewed = false };

        _mockMatchRepository.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(match);

        // Act
        var result = await _useCase.Execute(dto, userId);

        // Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        _mockReviewRepository.Verify(repo => repo.AddAsync(It.IsAny<Review>()), Times.Once);
        _mockMatchRepository.Verify(repo => repo.UpdateAsync(It.Is<SmartQuiz.Core.Entities.Match>(m => m.Reviewed == true && m.ReviewId != null)), Times.Once);
        Assert.NotNull((Guid)data.Id);
    }
}