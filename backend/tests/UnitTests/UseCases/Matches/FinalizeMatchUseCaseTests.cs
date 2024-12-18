using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.UseCases.Matches;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;
using Match = SmartQuiz.Core.Entities.Match;

namespace UnitTests.UseCases.Matches;

public class FinalizeMatchUseCaseTests
{
    private readonly Mock<IMatchService> _matchServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly FinalizeMatchUseCase _useCase;

    public FinalizeMatchUseCaseTests()
    {
        _matchServiceMock = new Mock<IMatchService>();
        _authServiceMock = new Mock<IAuthService>();
        _useCase = new FinalizeMatchUseCase(_matchServiceMock.Object, _authServiceMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenMatchDoesNotExist()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        _matchServiceMock.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync((Match)null);

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
        var match = new Match { Id = matchId, UserId = Guid.NewGuid(), Quiz = new Quiz { UserId = Guid.NewGuid()}}; // Outro usuário é o dono

        _matchServiceMock.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(match);
        _authServiceMock.Setup(service => service.ValidateSameUser(match.UserId, userId)).Throws<UnauthorizedAccessException>();

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
        var match = new Match { Id = matchId, UserId = userId, Status = EMatchStatus.Created, Quiz = new Quiz { UserId = Guid.NewGuid()}};

        _matchServiceMock.Setup(service => service.GetByIdAsync(matchId)).ReturnsAsync(match);
        _matchServiceMock.Setup(service => service.FinalizeMatch(It.IsAny<Match>())).Callback<Match>(m => m.Status = EMatchStatus.Finished);

        // Act
        var result = await _useCase.Execute(matchId, userId);

        // Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        _matchServiceMock.Verify(service => service.UpdateAsync(match), Times.Once);
        
        Assert.Equal(matchId, (Guid)data.Id);
        Assert.Equal(EMatchStatus.Finished, match.Status);
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidOperationException_WhenMatchIsAlreadyFinished()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var match = new Match { Id = matchId, UserId = userId, Status = EMatchStatus.Finished };

        _matchServiceMock.Setup(x => x.FinalizeMatch(It.IsAny<Match>())).Throws<InvalidOperationException>();
        _matchServiceMock.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(match);

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
        var match = new Match { Id = matchId, UserId = userId, Status = EMatchStatus.Failed, Quiz = new Quiz { UserId = Guid.NewGuid()}};
        
        _matchServiceMock.Setup(x => x.FinalizeMatch(It.IsAny<Match>())).Throws<InvalidOperationException>();
        _matchServiceMock.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(match);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.Execute(matchId, userId));
    }
}