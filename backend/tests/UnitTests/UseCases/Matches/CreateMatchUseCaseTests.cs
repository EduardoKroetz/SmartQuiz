using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.UseCases.Matches;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Matches;

public class CreateMatchUseCaseTests
{
    private readonly Mock<IMatchRepository> _matchRepositoryMock;
    private readonly Mock<IQuizRepository> _quizRepositoryMock;
    private readonly CreateMatchUseCase _useCase;

    public CreateMatchUseCaseTests()
    {
        _matchRepositoryMock = new Mock<IMatchRepository>();
        _quizRepositoryMock = new Mock<IQuizRepository>();
        _useCase = new CreateMatchUseCase(_matchRepositoryMock.Object, _quizRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_ValidInput_ReturnsResult()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var quiz = new Quiz { Id = Guid.NewGuid(), IsActive = true, ExpiresInSeconds = 120 };
        var nextQuestion = new Question { Id = Guid.NewGuid(), QuizId = quiz.Id, Order = 0, Text = "", AnswerOptions = [] };

        _quizRepositoryMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

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
        _quizRepositoryMock.Setup(x => x.GetByIdAsync(quizId)).ReturnsAsync((Quiz)null);

        //Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(quizId, userId));
    }

    [Fact]
    public async Task Execute_InactiveQuiz_ThrowsInvalidOperationException()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var quiz = new Quiz { Id = Guid.NewGuid(), IsActive = false };

        _quizRepositoryMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Execute(quiz.Id, userId));
    }
}
