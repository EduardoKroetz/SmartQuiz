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

public class CreateMatchUseCaseTests
{
    private readonly Mock<IMatchService> _matchServiceMock;
    private readonly Mock<IQuizService> _quizServiceMock;
    private readonly CreateMatchUseCase _useCase;

    public CreateMatchUseCaseTests()
    {
        _matchServiceMock = new Mock<IMatchService>();
        _quizServiceMock = new Mock<IQuizService>();
        _useCase = new CreateMatchUseCase(_quizServiceMock.Object, _matchServiceMock.Object);
    }

    [Fact]
    public async Task Execute_ValidInput_ReturnsResult()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var quiz = new Quiz { Id = Guid.NewGuid(), IsActive = true, ExpiresInSeconds = 120 };
        var match = new Match { Id = Guid.NewGuid(), Quiz = quiz, QuizId = quiz.Id, UserId = userId, Status = EMatchStatus.Created };
        
        _matchServiceMock.Setup(x => x.CreateMatch(userId, quiz.Id)).Returns(match);
        _quizServiceMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);
        
        //Act
        var result = await _useCase.Execute(quiz.Id, userId);

        //Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));

        Assert.NotNull(data);
        Assert.True(quiz.IsActive);
    }


    [Fact]
    public async Task Execute_QuizNotFound_ThrowsNotFoundException()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var quizId = Guid.NewGuid();
        
        _quizServiceMock.Setup(x => x.GetByIdAsync(quizId)).ReturnsAsync((Quiz)null);

        //Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(quizId, userId));
    }

    [Fact]
    public async Task Execute_InactiveQuiz_ThrowsInvalidOperationException()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var quiz = new Quiz { Id = Guid.NewGuid(), IsActive = false };

        _quizServiceMock.Setup(x => x.VerifyQuizActivation(quiz)).Throws<InvalidOperationException>();
        _quizServiceMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Execute(quiz.Id, userId));
    }
}
