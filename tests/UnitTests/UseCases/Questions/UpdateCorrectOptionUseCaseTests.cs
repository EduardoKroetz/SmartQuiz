using Moq;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.UseCases.Questions;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Questions;

public class UpdateCorrectOptionUseCaseTests
{
    private readonly Mock<IQuestionRepository> _questionRepositoryMock;
    private readonly Mock<IAnswerOptionRepository> _answerOptionRepositoryMock;
    private readonly UpdateCorrectOptionUseCase _useCase;

    public UpdateCorrectOptionUseCaseTests()
    {
        _questionRepositoryMock = new Mock<IQuestionRepository>();
        _answerOptionRepositoryMock = new Mock<IAnswerOptionRepository>();
        _useCase = new UpdateCorrectOptionUseCase(_questionRepositoryMock.Object, _answerOptionRepositoryMock.Object);
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
            Options = new List<AnswerOption> { currentCorrectOption, newCorrectOption },
            Quiz = new Quiz { Id = Guid.NewGuid(), UserId = userId, }
        };

        _questionRepositoryMock.Setup(x => x.GetAsync(question.Id, true)).ReturnsAsync(question);

        //Act
        var result = await _useCase.Execute(question.Id, newCorrectOption.Id, userId);

        //Assert
        Assert.NotNull(result);
        Assert.False(currentCorrectOption.IsCorrectOption);
        Assert.True(newCorrectOption.IsCorrectOption);

        _answerOptionRepositoryMock.Verify(r => r.UpdateAsync(currentCorrectOption), Times.Once);
        _answerOptionRepositoryMock.Verify(r => r.UpdateAsync(newCorrectOption), Times.Once);
    }

    [Fact]
    public async Task Execute_QuestionNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var newCorrectOptionId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _questionRepositoryMock.Setup(r => r.GetAsync(questionId, true)).ReturnsAsync((Question)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(questionId, newCorrectOptionId, userId));
    }

    [Fact]
    public async Task Execute_UserDoesNotHavePermission_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var newCorrectOptionId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var quizOwnerId = Guid.NewGuid();

        var question = new Question
        {
            Id = questionId,
            Quiz = new Quiz { UserId = quizOwnerId },
            Options = new List<AnswerOption>
            {
                new AnswerOption { Id = newCorrectOptionId }
            }
        };

        _questionRepositoryMock.Setup(r => r.GetAsync(questionId, true)).ReturnsAsync(question);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _useCase.Execute(questionId, newCorrectOptionId, userId));
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
            Options = new List<AnswerOption>
            {
                new AnswerOption { Id = Guid.NewGuid() } // Não contém newCorrectOptionId
            }
        };

        _questionRepositoryMock.Setup(r => r.GetAsync(questionId, true)).ReturnsAsync(question);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(questionId, newCorrectOptionId, userId));
    }


}
