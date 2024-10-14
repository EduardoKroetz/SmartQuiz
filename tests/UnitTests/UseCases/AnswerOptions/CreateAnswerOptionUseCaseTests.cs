using Moq;
using Newtonsoft.Json;
using QuizDev.Application.Exceptions;
using QuizDev.Application.UseCases.AnswerOptions;
using QuizDev.Core.DTOs.AnswerOptions;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace UnitTests.UseCases.AnswerOptions;


public class CreateAnswerOptionUseCaseTests
{
    private readonly Mock<IAnswerOptionRepository> _mockAnswerOptionRepository;
    private readonly Mock<IQuestionRepository> _mockQuestionRepository;
    private readonly CreateAnswerOptionUseCase _useCase;

    public CreateAnswerOptionUseCaseTests()
    {
        _mockAnswerOptionRepository = new Mock<IAnswerOptionRepository>();
        _mockQuestionRepository = new Mock<IQuestionRepository>();
        _useCase = new CreateAnswerOptionUseCase(_mockAnswerOptionRepository.Object, _mockQuestionRepository.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenQuestionDoesNotExist()
    {
        // Arrange
        var createAnswerOption = new CreateAnswerOptionDto { QuestionId = Guid.NewGuid() };
        _mockQuestionRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), true)).ReturnsAsync((Question)null);

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

        _mockQuestionRepository.Setup(repo => repo.GetAsync(createAnswerOption.QuestionId, true)).ReturnsAsync(question);

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
            Options = new List<AnswerOption>
            {
                new AnswerOption { IsCorrectOption = true }
            }
        };

        _mockQuestionRepository.Setup(repo => repo.GetAsync(createAnswerOption.QuestionId, true)).ReturnsAsync(question);

        // Act
        await _useCase.Execute(createAnswerOption, userId);

        // Assert
        _mockAnswerOptionRepository.Verify(repo => repo.UpdateAsync(It.IsAny<AnswerOption>()), Times.Once);
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
            Options = new List<AnswerOption>()
        };

        _mockQuestionRepository.Setup(repo => repo.GetAsync(createAnswerOption.QuestionId, true )).ReturnsAsync(question);

        // Act
        var result = await _useCase.Execute(createAnswerOption, userId);

        // Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        _mockAnswerOptionRepository.Verify(repo => repo.CreateAsync(It.Is<AnswerOption>(ao =>
            ao.QuestionId == createAnswerOption.QuestionId &&
            ao.IsCorrectOption == createAnswerOption.IsCorrectOption &&
            ao.Response == createAnswerOption.Response)), Times.Once);

        Assert.Equal(question.Id, (Guid)data.QuestionId);
    }
}
