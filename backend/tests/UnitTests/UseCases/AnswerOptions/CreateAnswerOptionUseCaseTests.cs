using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.UseCases.AnswerOptions;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;

namespace UnitTests.UseCases.AnswerOptions;


public class CreateAnswerOptionUseCaseTests
{
    private readonly Mock<IAnswerOptionService> _answerOptionServiceMock;
    private readonly Mock<IQuestionService> _questionServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly CreateAnswerOptionUseCase _useCase;

    public CreateAnswerOptionUseCaseTests()
    {
        _answerOptionServiceMock = new Mock<IAnswerOptionService>();
        _questionServiceMock = new Mock<IQuestionService>();
        _authServiceMock = new Mock<IAuthService>();
        _useCase = new CreateAnswerOptionUseCase(_answerOptionServiceMock.Object, _questionServiceMock.Object, _authServiceMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenQuestionDoesNotExist()
    {
        // Arrange
        var createAnswerOption = new CreateAnswerOptionDto { QuestionId = Guid.NewGuid() };
        _questionServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Question)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _useCase.Execute(createAnswerOption, Guid.NewGuid()));
    }

    [Fact]
    public async Task Execute_ShouldThrowUnauthorizedAccessException_WhenUserIsNotOwnerOfQuiz()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var createAnswerOption = new CreateAnswerOptionDto { QuestionId = Guid.NewGuid() };

        var question = new Question
        {
            Id = createAnswerOption.QuestionId,
            Quiz = new Quiz { UserId = Guid.NewGuid() } // Outro usuário
        };

        _authServiceMock.Setup(service => service.ValidateSameUser(question.Quiz.UserId, userId)).Throws<UnauthorizedAccessException>();
        _questionServiceMock.Setup(service => service.GetByIdAsync(createAnswerOption.QuestionId)).ReturnsAsync(question);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _useCase.Execute(createAnswerOption, userId));
    }

    [Fact]
    public async Task Execute_ShouldUpdateCorrectAnswerOption_WhenCreatingANewCorrectOption()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var createAnswerOption = new CreateAnswerOptionDto
        {
            QuestionId = Guid.NewGuid(),
            IsCorrectOption = true
        };

        var question = new Question
        {
            Id = createAnswerOption.QuestionId,
            Quiz = new Quiz { UserId = userId },
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption { IsCorrectOption = true }
            }
        };

        _questionServiceMock.Setup(service => service.GetByIdAsync(createAnswerOption.QuestionId)).ReturnsAsync(question);
        _answerOptionServiceMock.Setup(service => service.UpdateCorrectOption(question));
        _answerOptionServiceMock.Setup(service => service.CreateAnswerOption(createAnswerOption)).Returns(new AnswerOption { Id = Guid.NewGuid() });
        
        // Act
        await _useCase.Execute(createAnswerOption, userId);

        // Assert
        _answerOptionServiceMock.Verify(service => service.UpdateCorrectOption(question), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldCreateNewAnswerOption_WhenDataIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var createAnswerOption = new CreateAnswerOptionDto
        {
            QuestionId = Guid.NewGuid(),
            IsCorrectOption = false,
            Response = "Resposta A"
        };

        var question = new Question
        {
            Id = createAnswerOption.QuestionId,
            Quiz = new Quiz { UserId = userId },
            AnswerOptions = new List<AnswerOption>()
        };

        _authServiceMock.Setup(validator => validator.ValidateSameUser(question.Quiz.UserId, userId));
        _questionServiceMock.Setup(service => service.GetByIdAsync(createAnswerOption.QuestionId)).ReturnsAsync(question);
        _answerOptionServiceMock.Setup(service => service.CreateAnswerOption(createAnswerOption)).Returns(new AnswerOption { Id = Guid.NewGuid() });

        // Act
        var result = await _useCase.Execute(createAnswerOption, userId);

        // Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));

        Assert.Equal(question.Id, (Guid)data.QuestionId);
    }
}
