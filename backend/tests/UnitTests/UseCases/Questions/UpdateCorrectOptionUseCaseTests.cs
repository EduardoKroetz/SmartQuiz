using Moq;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.UseCases.Questions;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Questions;

public class UpdateCorrectOptionUseCaseTests
{
    private readonly Mock<IQuestionService> _questionServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IAnswerOptionService> _answerOptionServiceMock;
    private readonly UpdateCorrectOptionUseCase _useCase;

    public UpdateCorrectOptionUseCaseTests()
    {
        _questionServiceMock = new Mock<IQuestionService>();
        _authServiceMock = new Mock<IAuthService>();
        _answerOptionServiceMock = new Mock<IAnswerOptionService>();
        _useCase = new UpdateCorrectOptionUseCase(_questionServiceMock.Object, _answerOptionServiceMock.Object ,_authServiceMock.Object);
    }

    [Fact]
    public async Task Execute_ValidInputs_ReturnsResult()
    {
        //Arrange
        var userId = Guid.NewGuid();

        var currentCorrectOption = new AnswerOption { Id = Guid.NewGuid(), IsCorrectOption = true };
        var newCorrectOption = new AnswerOption { Id = Guid.NewGuid(), IsCorrectOption = false };

        var question = new Question
        {
            Id = Guid.NewGuid(),
            AnswerOptions = new List<AnswerOption> { currentCorrectOption, newCorrectOption },
            Quiz = new Quiz { Id = Guid.NewGuid(), UserId = userId, }
        };

        _answerOptionServiceMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(newCorrectOption);
        _questionServiceMock.Setup(x =>
            x.UpdateCorrectOption(It.IsAny<Question>(), It.IsAny<Guid>())).Callback(() =>
        {
            currentCorrectOption.IsCorrectOption = false;
            newCorrectOption.IsCorrectOption = true;
        });
        _questionServiceMock.Setup(x => x.GetByIdAsync(question.Id)).ReturnsAsync(question);

        //Act
        var result = await _useCase.Execute(question.Id, newCorrectOption.Id, userId);

        //Assert
        Assert.NotNull(result);
        Assert.False(currentCorrectOption.IsCorrectOption);
        Assert.True(newCorrectOption.IsCorrectOption);
    }

    [Fact]
    public async Task Execute_QuestionNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var newCorrectOption = new AnswerOption { Id = Guid.NewGuid(), IsCorrectOption = true };
        var userId = Guid.NewGuid();

        _answerOptionServiceMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(newCorrectOption);
        _questionServiceMock.Setup(r => r.GetByIdAsync(questionId)).ReturnsAsync((Question)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(questionId, newCorrectOption.Id, userId));
    }

    [Fact]
    public async Task Execute_UserDoesNotHavePermission_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var newCorrectOption = new AnswerOption { Id = Guid.NewGuid(), IsCorrectOption = true };
        var userId = Guid.NewGuid();
        var quizOwnerId = Guid.NewGuid();

        var question = new Question
        {
            Id = questionId,
            Quiz = new Quiz { UserId = quizOwnerId },
            AnswerOptions = new List<AnswerOption>
            {
                newCorrectOption
            }
        };

        _answerOptionServiceMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(newCorrectOption);
        _authServiceMock.Setup(x => x.ValidateSameUser(question.Quiz.UserId, userId))
            .Throws<UnauthorizedAccessException>();
        _questionServiceMock.Setup(r => r.GetByIdAsync(questionId)).ReturnsAsync(question);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _useCase.Execute(questionId, newCorrectOption.Id, userId));
    }

    [Fact]
    public async Task Execute_NewCorrectOptionNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var newCorrectOptionId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var question = new Question
        {
            Id = questionId,
            Quiz = new Quiz { UserId = userId },
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption { Id = Guid.NewGuid() } // Não contém newCorrectOptionId
            }
        };

        _answerOptionServiceMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((AnswerOption)null);
        _questionServiceMock.Setup(r => r.GetByIdAsync(questionId)).ReturnsAsync(question);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(questionId, newCorrectOptionId, userId));
    }


}
