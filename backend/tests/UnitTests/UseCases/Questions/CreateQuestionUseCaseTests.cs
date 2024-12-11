using Moq;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.UseCases.Questions;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Questions;

public class CreateQuestionUseCaseTests
{
    private readonly Mock<IQuestionRepository> _questionRepositoryMock;
    private readonly Mock<IQuizRepository> _quizRepositoryMock;
    private readonly Mock<IAnswerOptionRepository> _answerOptionRepositoryMock;
    private readonly CreateQuestionUseCase _useCase;

    public CreateQuestionUseCaseTests()
    {
        _questionRepositoryMock = new Mock<IQuestionRepository>();
        _quizRepositoryMock = new Mock<IQuizRepository>();
        _answerOptionRepositoryMock = new Mock<IAnswerOptionRepository>();
        _useCase = new CreateQuestionUseCase(_questionRepositoryMock.Object, _quizRepositoryMock.Object, _answerOptionRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_ValidInputs_ReturnsResult()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var quiz = new Quiz
        {
            Id = Guid.NewGuid(),
            Questions = new List<Question>(),
            UserId = userId
        };
        var createQuestionDto = new CreateQuestionDto
        {
            QuizId = quiz.Id,
            Order = 0,
            Text = "Teste?",
            Options = new List<CreateAnswerOptionInQuestionDto>()
            {
                new () { IsCorrectOption = true },
                new () { IsCorrectOption = false }
            }
        };

        _quizRepositoryMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

        //Act
        var result = await _useCase.Execute(createQuestionDto, userId);

        //Assert
        _questionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Question>()), Times.Once);
        _answerOptionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<AnswerOption>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Execute_QuizNotExists_ThrowsNotFoundException()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var createQuestionDto = new CreateQuestionDto { QuizId = Guid.NewGuid() };

        _quizRepositoryMock.Setup(x => x.GetByIdAsync(createQuestionDto.QuizId)).ReturnsAsync((Quiz)null);

        //Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(createQuestionDto, userId));
    }


    [Fact]
    public async Task Execute_NoOptions_ReturnsResult()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var quiz = new Quiz { Id = Guid.NewGuid(), Questions = new List<Question>(), UserId = userId };
        var createQuestionDto = new CreateQuestionDto { QuizId = quiz.Id, Order = 0, Text = "Teste?", Options = new List<CreateAnswerOptionInQuestionDto>() { } };

        _quizRepositoryMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);


        //Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(createQuestionDto, userId));
    }

    [Fact]
    public async Task Execute_NoCorrectOptions_ThrowsArgumentException()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var quiz = new Quiz { Id = Guid.NewGuid(), Questions = new List<Question>(), UserId = userId };
        var createQuestionDto = new CreateQuestionDto
        {
            QuizId = quiz.Id,
            Order = 0,
            Text = "Teste?",
            Options = new List<CreateAnswerOptionInQuestionDto>()
            {
                new() { IsCorrectOption = false },
                new() { IsCorrectOption = false }
            }
        };

        _quizRepositoryMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

        //Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(createQuestionDto, userId));
    }

    [Fact]
    public async Task Execute_WithManyCorrectOptions_ThrowsArgumentException()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var quiz = new Quiz { Id = Guid.NewGuid(), Questions = new List<Question>(), UserId = userId };
        var createQuestionDto = new CreateQuestionDto
        {
            QuizId = quiz.Id,
            Order = 0,
            Text = "Teste?",
            Options = new List<CreateAnswerOptionInQuestionDto>()
            {
                new() { IsCorrectOption = true },
                new() { IsCorrectOption = true }
            }
        };

        _quizRepositoryMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

        //Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(createQuestionDto, userId));
    }

    [Fact]
    public async Task Execute_InvalidOrder_ThrowsArgumentException()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var quiz = new Quiz { Id = Guid.NewGuid(), Questions = new List<Question>(), UserId = userId };
        var createQuestionDto = new CreateQuestionDto
        {
            QuizId = quiz.Id,
            Order = 10,
            Text = "Teste?",
            Options = new List<CreateAnswerOptionInQuestionDto>()
            {
                new() { IsCorrectOption = true },
                new() { IsCorrectOption = false }
            }
        };

        _quizRepositoryMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

        //Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(createQuestionDto, userId));
        _questionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Question>()), Times.Never);
    }

    [Fact]
    public async Task Execute_NotTheUserWhoCreatedTheQuiz_ThrowsUnauthorizedException()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var quiz = new Quiz { Id = Guid.NewGuid(), Questions = new List<Question>(), UserId = Guid.NewGuid() };
        var createQuestionDto = new CreateQuestionDto
        {
            QuizId = quiz.Id,
            Order = 10,
            Text = "Teste?",
            Options = new List<CreateAnswerOptionInQuestionDto>()
            {
                new() { IsCorrectOption = true },
                new() { IsCorrectOption = false }
            }
        };

        _quizRepositoryMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

        //Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _useCase.Execute(createQuestionDto, userId));
        _questionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Question>()), Times.Never);
    }
}
