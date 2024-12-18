using Moq;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.UseCases.Questions;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Questions;

public class CreateQuestionUseCaseTests
{
    private readonly Mock<IQuestionService> _questionServiceMock;
    private readonly Mock<IQuizService> _quizServiceMock;
    private readonly Mock<IAnswerOptionService> _answerOptionServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly CreateQuestionUseCase _useCase;

    public CreateQuestionUseCaseTests()
    {
        _questionServiceMock = new Mock<IQuestionService>();
        _quizServiceMock = new Mock<IQuizService>();
        _answerOptionServiceMock = new Mock<IAnswerOptionService>();
        _authServiceMock = new Mock<IAuthService>();
        _useCase = new CreateQuestionUseCase(_quizServiceMock.Object, _questionServiceMock.Object ,_answerOptionServiceMock.Object, _authServiceMock.Object);
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
        var question = new Question{ QuizId = quiz.Id, Quiz = quiz, Order = 0, Text = createQuestionDto.Text };

        _questionServiceMock.Setup(x => x.CreateQuestion(It.IsAny<CreateQuestionDto>())).Returns(question);
        _quizServiceMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

        //Act
        var result = await _useCase.Execute(createQuestionDto, userId);

        //Assert
        _questionServiceMock.Verify(x => x.AddAsync(It.IsAny<Question>()), Times.Once);
        _answerOptionServiceMock.Verify(x => x.AddAsync(It.IsAny<AnswerOption>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Execute_QuizNotExists_ThrowsNotFoundException()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var createQuestionDto = new CreateQuestionDto
        {
            QuizId = Guid.NewGuid(),
            Order = 0,
            Text = "Teste?",
            Options = new List<CreateAnswerOptionInQuestionDto>()
            {
                new () { IsCorrectOption = true },
                new () { IsCorrectOption = false }
            }
        };

        _quizServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()));

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

        _quizServiceMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

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

        _quizServiceMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

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

        _quizServiceMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

        //Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(createQuestionDto, userId));
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

        _authServiceMock.Setup(x => x.ValidateSameUser(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws<UnauthorizedAccessException>();
        _quizServiceMock.Setup(x => x.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

        //Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _useCase.Execute(createQuestionDto, userId));
        _questionServiceMock.Verify(x => x.AddAsync(It.IsAny<Question>()), Times.Never);
    }
}
