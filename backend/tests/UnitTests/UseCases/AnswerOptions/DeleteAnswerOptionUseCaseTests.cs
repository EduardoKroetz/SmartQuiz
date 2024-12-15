using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.UseCases.AnswerOptions;
using SmartQuiz.Application.Validators.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.AnswerOptions;

public class DeleteAnswerOptionUseCaseTests
{
    private readonly Mock<IAnswerOptionService> _answerOptionServiceMock;
    private readonly Mock<IQuestionService> _questionServiceMock;
    private readonly Mock<IUserAuthorizationValidator> _userAuthorizationValidatorMock;
    private readonly DeleteAnswerOptionUseCase _useCase;

    public DeleteAnswerOptionUseCaseTests()
    {
        _answerOptionServiceMock = new Mock<IAnswerOptionService>();
        _questionServiceMock = new Mock<IQuestionService>();
        _userAuthorizationValidatorMock = new Mock<IUserAuthorizationValidator>();
        _useCase = new DeleteAnswerOptionUseCase(_answerOptionServiceMock.Object, _questionServiceMock.Object, _userAuthorizationValidatorMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenAnswerOptionDoesNotExist()
    {
        // Arrange
        var answerOptionId = Guid.NewGuid();
        _answerOptionServiceMock.Setup(service => service.GetByIdAsync(answerOptionId)).ReturnsAsync((AnswerOption)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _useCase.Execute(answerOptionId, Guid.NewGuid()));
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenQuestionDoesNotExist()
    {
        // Arrange
        var answerOptionId = Guid.NewGuid();
        var answerOption = new AnswerOption { Id = answerOptionId, QuestionId = Guid.NewGuid() };

        _answerOptionServiceMock.Setup(service => service.GetByIdAsync(answerOptionId)).ReturnsAsync(answerOption);
        _questionServiceMock.Setup(service => service.GetByIdAsync(answerOption.QuestionId)).ReturnsAsync((Question)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _useCase.Execute(answerOptionId, Guid.NewGuid()));
    }

    [Fact]
    public async Task Execute_ShouldThrowUnauthorizedAccessException_WhenUserIsNotOwnerOfQuiz()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var answerOptionId = Guid.NewGuid();
        var answerOption = new AnswerOption { Id = answerOptionId, QuestionId = Guid.NewGuid() };

        var question = new Question
        {
            Id = answerOption.QuestionId,
            Quiz = new Quiz { UserId = Guid.NewGuid() } // Outro usuário
        };

        _answerOptionServiceMock.Setup(service => service.GetByIdAsync(answerOptionId)).ReturnsAsync(answerOption);
        _questionServiceMock.Setup(service => service.GetByIdAsync(answerOption.QuestionId)).ReturnsAsync(question);
        _userAuthorizationValidatorMock.Setup(validator => validator.ValidateAuthorization(question.Quiz.UserId, userId)).Throws<UnauthorizedAccessException>();

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _useCase.Execute(answerOptionId, userId));
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidOperationException_WhenQuestionHasLessThanTwoAnswerOptions()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var answerOptionId = Guid.NewGuid();
        var answerOption = new AnswerOption { Id = answerOptionId, QuestionId = Guid.NewGuid() };

        var question = new Question
        {
            Id = answerOption.QuestionId,
            Quiz = new Quiz { UserId = userId },
            AnswerOptions = new List<AnswerOption> { new AnswerOption(), new AnswerOption() } // Exatamente 2 opções
        };
    
        _answerOptionServiceMock.Setup(service => service.DeleteAsync(answerOption, question)).Throws<InvalidOperationException>();
        _answerOptionServiceMock.Setup(service => service.GetByIdAsync(answerOptionId)).ReturnsAsync(answerOption);
        _questionServiceMock.Setup(service => service.GetByIdAsync(answerOption.QuestionId)).ReturnsAsync(question);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.Execute(answerOptionId, userId));
    }

    [Fact]
    public async Task Execute_ShouldDeleteAnswerOption_WhenDataIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var answerOptionId = Guid.NewGuid();
        var answerOption = new AnswerOption { Id = answerOptionId, QuestionId = Guid.NewGuid() };

        var question = new Question
        {
            Id = answerOption.QuestionId,
            Quiz = new Quiz { UserId = userId },
            AnswerOptions = new List<AnswerOption>
            {
                new AnswerOption(), new AnswerOption(), new AnswerOption() // Mais de 2 opções
            }
        };

        _answerOptionServiceMock.Setup(service => service.GetByIdAsync(answerOptionId)).ReturnsAsync(answerOption);
        _questionServiceMock.Setup(service => service.GetByIdAsync(answerOption.QuestionId)).ReturnsAsync(question);

        // Act
        var result = await _useCase.Execute(answerOptionId, userId);

        // Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        _answerOptionServiceMock.Verify(service => service.DeleteAsync(answerOption, question), Times.Once);
        Assert.Equal(answerOption.Id, (Guid)data.AnswerOptionId);
        Assert.Equal(question.Id, (Guid)data.QuestionId);
    }
}