using Moq;
using Newtonsoft.Json;
using QuizDev.Application.Exceptions;
using QuizDev.Application.UseCases.AnswerOptions;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace UnitTests.UseCases.AnswerOptions;

public class DeleteAnswerOptionUseCaseTests
{
    private readonly Mock<IAnswerOptionRepository> _mockAnswerOptionRepository;
    private readonly Mock<IQuestionRepository> _mockQuestionRepository;
    private readonly DeleteAnswerOptionUseCase _useCase;

    public DeleteAnswerOptionUseCaseTests()
    {
        _mockAnswerOptionRepository = new Mock<IAnswerOptionRepository>();
        _mockQuestionRepository = new Mock<IQuestionRepository>();
        _useCase = new DeleteAnswerOptionUseCase(_mockAnswerOptionRepository.Object, _mockQuestionRepository.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenAnswerOptionDoesNotExist()
    {
        // Arrange
        var answerOptionId = Guid.NewGuid();
        _mockAnswerOptionRepository.Setup(repo => repo.GetById(answerOptionId)).ReturnsAsync((AnswerOption)null);

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

        _mockAnswerOptionRepository.Setup(repo => repo.GetById(answerOptionId)).ReturnsAsync(answerOption);
        _mockQuestionRepository.Setup(repo => repo.GetAsync(answerOption.QuestionId, true)).ReturnsAsync((Question)null);

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

        _mockAnswerOptionRepository.Setup(repo => repo.GetById(answerOptionId)).ReturnsAsync(answerOption);
        _mockQuestionRepository.Setup(repo => repo.GetAsync(answerOption.QuestionId, true)).ReturnsAsync(question);

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
            Options = new List<AnswerOption> { new AnswerOption(), new AnswerOption() } // Exatamente 2 opções
        };

        _mockAnswerOptionRepository.Setup(repo => repo.GetById(answerOptionId)).ReturnsAsync(answerOption);
        _mockQuestionRepository.Setup(repo => repo.GetAsync(answerOption.QuestionId, true)).ReturnsAsync(question);

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
            Options = new List<AnswerOption>
            {
                new AnswerOption(), new AnswerOption(), new AnswerOption() // Mais de 2 opções
            }
        };

        _mockAnswerOptionRepository.Setup(repo => repo.GetById(answerOptionId)).ReturnsAsync(answerOption);
        _mockQuestionRepository.Setup(repo => repo.GetAsync(answerOption.QuestionId, true)).ReturnsAsync(question);

        // Act
        var result = await _useCase.Execute(answerOptionId, userId);

        // Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        _mockAnswerOptionRepository.Verify(repo => repo.DeleteAsync(answerOption), Times.Once);
        Assert.Equal(answerOption.Id, (Guid)data.AnswerOptionId);
        Assert.Equal(question.Id, (Guid)data.QuestionId);
    }
}