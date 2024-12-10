using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.UseCases.Matches;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Matches;

public class FinalizeMatchUseCaseTests
{
    private readonly Mock<IMatchRepository> _mockMatchRepository;
    private readonly FinalizeMatchUseCase _useCase;

    public FinalizeMatchUseCaseTests()
    {
        _mockMatchRepository = new Mock<IMatchRepository>();
        _useCase = new FinalizeMatchUseCase(_mockMatchRepository.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenMatchDoesNotExist()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        _mockMatchRepository.Setup(repo => repo.GetAsync(matchId)).ReturnsAsync((SmartQuiz.Core.Entities.Match)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _useCase.Execute(matchId, Guid.NewGuid()));
    }

    [Fact]
    public async Task Execute_ShouldThrowUnauthorizedAccessException_WhenUserIsNotMatchOwner()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var match = new SmartQuiz.Core.Entities.Match { Id = matchId, UserId = Guid.NewGuid() }; // Outro usuário é o dono

        _mockMatchRepository.Setup(repo => repo.GetAsync(matchId)).ReturnsAsync(match);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _useCase.Execute(matchId, userId));
    }

    [Fact]
    public async Task Execute_ShouldFinalizeMatch_WhenUserIsOwnerAndMatchExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var match = new SmartQuiz.Core.Entities.Match { Id = matchId, UserId = userId, Status = EMatchStatus.Created };

        _mockMatchRepository.Setup(repo => repo.GetAsync(matchId)).ReturnsAsync(match);

        // Act
        var result = await _useCase.Execute(matchId, userId);

        // Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        _mockMatchRepository.Verify(repo => repo.UpdateAsync(match), Times.Once);
        Assert.Equal(matchId, (Guid)data.Id);
        Assert.Equal(EMatchStatus.Finished, match.Status);
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidOperationException_WhenMatchIsAlreadyFinished()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var match = new SmartQuiz.Core.Entities.Match { Id = matchId, UserId = userId, Status = EMatchStatus.Finished };

        _mockMatchRepository.Setup(repo => repo.GetAsync(matchId)).ReturnsAsync(match);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.Execute(matchId, userId));
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidOperationException_WhenMatchHasFailedStatus()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var match = new SmartQuiz.Core.Entities.Match { Id = matchId, UserId = userId, Status = EMatchStatus.Failed };

        _mockMatchRepository.Setup(repo => repo.GetAsync(matchId)).ReturnsAsync(match);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.Execute(matchId, userId));
    }
}