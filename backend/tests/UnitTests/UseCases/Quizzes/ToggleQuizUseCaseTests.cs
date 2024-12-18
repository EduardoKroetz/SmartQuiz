using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.UseCases.Quizzes;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Quizzes;

public class ToggleQuizUseCaseTests
{
    private readonly Mock<IQuizService> _quizServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly ToggleQuizUseCase _toggleQuizUseCase;

    public ToggleQuizUseCaseTests()
    {
        _quizServiceMock = new Mock<IQuizService>();
        _authServiceMock = new Mock<IAuthService>();
        _toggleQuizUseCase = new ToggleQuizUseCase(_quizServiceMock.Object, _authServiceMock.Object);
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

        _quizServiceMock.Setup(q => q.ToggleQuiz(It.IsAny<Quiz>())).Callback(() => quiz.IsActive = true);
        _quizServiceMock.Setup(q => q.GetByIdAsync(quizId)).ReturnsAsync(quiz);
        _quizServiceMock.Setup(q => q.UpdateAsync(It.IsAny<Quiz>())).Returns(Task.CompletedTask);

        // Act
        var result = await _toggleQuizUseCase.Execute(quizId, userId);

        // Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        Assert.NotNull(result);
        Assert.Equal(quizId, (Guid)data.QuizId);
        Assert.True(quiz.IsActive);
        _quizServiceMock.Verify(q => q.UpdateAsync(quiz), Times.Once);
    }

    [Fact]
    public async Task Execute_QuizNotExists_ThrowsNotFoundException()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _quizServiceMock.Setup(q => q.GetByIdAsync(quizId)).ReturnsAsync((Quiz)null);

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
        
        _authServiceMock.Setup(x => x.ValidateSameUser(quiz.UserId, userId)).Throws<UnauthorizedAccessException>();
        _quizServiceMock.Setup(q => q.GetByIdAsync(quizId)).ReturnsAsync(quiz);

        //Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _toggleQuizUseCase.Execute(quizId, userId));
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

        _quizServiceMock.Setup(q => q.ToggleQuiz(It.IsAny<Quiz>())).Throws<ArgumentException>();
        _quizServiceMock.Setup(q => q.GetByIdAsync(quizId)).ReturnsAsync(quiz);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _toggleQuizUseCase.Execute(quizId, userId));
    }
}