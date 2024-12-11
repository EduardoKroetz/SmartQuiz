using AutoMapper;
using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.UseCases.AnswerOptions;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.DTOs.AutoMapper;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.AnswerOptions;


public class CreateAnswerOptionUseCaseTests
{
    private readonly Mock<IAnswerOptionRepository> _mockAnswerOptionRepository;
    private readonly Mock<IQuestionRepository> _mockQuestionRepository;
    private readonly IMapper _mapper;
    private readonly CreateAnswerOptionUseCase _useCase;

    public CreateAnswerOptionUseCaseTests()
    {
        _mockAnswerOptionRepository = new Mock<IAnswerOptionRepository>();
        _mockQuestionRepository = new Mock<IQuestionRepository>();
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }).CreateMapper();
        _useCase = new CreateAnswerOptionUseCase(_mockAnswerOptionRepository.Object, _mockQuestionRepository.Object, _mapper);
    }

    [Fact]
    public async Task Execute_ShouldThrowNotFoundException_WhenQuestionDoesNotExist()
    {
        // Arrange
        var createAnswerOption = new CreateAnswerOptionDto { QuestionId = Guid.NewGuid() };
        _mockQuestionRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Question)null);

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

        _mockQuestionRepository.Setup(repo => repo.GetByIdAsync(createAnswerOption.QuestionId)).ReturnsAsync(question);

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

        _mockQuestionRepository.Setup(repo => repo.GetByIdAsync(createAnswerOption.QuestionId)).ReturnsAsync(question);

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
            AnswerOptions = new List<AnswerOption>()
        };

        _mockQuestionRepository.Setup(repo => repo.GetByIdAsync(createAnswerOption.QuestionId)).ReturnsAsync(question);

        // Act
        var result = await _useCase.Execute(createAnswerOption, userId);

        // Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        _mockAnswerOptionRepository.Verify(repo => repo.AddAsync(It.Is<AnswerOption>(ao =>
            ao.QuestionId == createAnswerOption.QuestionId &&
            ao.IsCorrectOption == createAnswerOption.IsCorrectOption &&
            ao.Response == createAnswerOption.Response)), Times.Once);

        Assert.Equal(question.Id, (Guid)data.QuestionId);
    }
}
