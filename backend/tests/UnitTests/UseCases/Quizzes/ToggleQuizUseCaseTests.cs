using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.UseCases.Quizzes;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Quizzes;

public class ToggleQuizUseCaseTests
{
    private readonly Mock<IQuizRepository> _quizRepositoryMock;
    private readonly ToggleQuizUseCase _toggleQuizUseCase;

    public ToggleQuizUseCaseTests()
    {
        _quizRepositoryMock = new Mock<IQuizRepository>();
        _toggleQuizUseCase = new ToggleQuizUseCase(_quizRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_QuizExists_UserHasAccess_HasQuestions_ToggleQuizAndReturnsResult()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var quiz = new Quiz
        {
            Id = quizId,
            UserId = userId,
            IsActive = false,
            Questions = new List<Question>()
            {
                new Question()
            }
        };

        _quizRepositoryMock.Setup(q => q.GetAsync(quizId, true)).ReturnsAsync(quiz);
        _quizRepositoryMock.Setup(q => q.UpdateAsync(It.IsAny<Quiz>())).Returns(Task.CompletedTask);

        // Act
        var result = await _toggleQuizUseCase.Execute(quizId, userId);

        // Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        Assert.NotNull(result);
        Assert.Equal(quizId, (Guid)data.QuizId);
        Assert.True(quiz.IsActive);
        _quizRepositoryMock.Verify(q => q.UpdateAsync(quiz), Times.Once);
    }

    [Fact]
    public async Task Execute_QuizNotExists_ThrowsNotFoundException()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _quizRepositoryMock.Setup(q => q.GetAsync(quizId, true)).ReturnsAsync((Quiz)null);

        //Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _toggleQuizUseCase.Execute(quizId, userId));
    }

    [Fact]
    public async Task Execute_UserDoesNotHaveAccess_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var quiz = new Quiz
        {
            Id = quizId,
            UserId = Guid.NewGuid(), // Usuário diferente
            IsActive = false,
            Questions = new List<Question>()
        };

        _quizRepositoryMock.Setup(q => q.GetAsync(quizId, true)).ReturnsAsync(quiz);

        //Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _toggleQuizUseCase.Execute(quizId, Guid.NewGuid()));
    }

    [Fact]
    public async Task Execute_QuizIsInactiveAndHasNoQuestions_ThrowsArgumentException()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var quiz = new Quiz
        {
            Id = quizId,
            UserId = userId,
            IsActive = false,
            Questions = new List<Question>()
        };

        _quizRepositoryMock.Setup(q => q.GetAsync(quizId, true)).ReturnsAsync(quiz);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _toggleQuizUseCase.Execute(quizId, userId));
    }
}