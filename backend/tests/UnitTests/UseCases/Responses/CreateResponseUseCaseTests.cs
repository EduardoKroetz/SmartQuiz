using Moq;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.UseCases.Responses;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using Match = SmartQuiz.Core.Entities.Match;

namespace UnitTests.UseCases.Responses;

public class CreateResponseUseCaseTests
{
    private readonly Mock<IAnswerOptionService> _answerOptionServiceMock;
    private readonly Mock<IResponseService> _responseServiceMock;
    private readonly Mock<IMatchService> _matchServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly CreateResponseUseCase _useCase;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly Question _question = new() { Id = Guid.NewGuid() };
    private readonly AnswerOption _answerOption;
    private readonly Quiz _quiz;
    private readonly SmartQuiz.Core.Entities.Match _match;
    private readonly Response _response;

    public CreateResponseUseCaseTests()
    {
        // Inicialização dos mocks
        _answerOptionServiceMock = new Mock<IAnswerOptionService>();
        _responseServiceMock = new Mock<IResponseService>();
        _matchServiceMock = new Mock<IMatchService>();
        _authServiceMock = new Mock<IAuthService>();

        _useCase = new CreateResponseUseCase(
            _answerOptionServiceMock.Object,
            _responseServiceMock.Object,
            _matchServiceMock.Object,
            _authServiceMock.Object);

        // Dados de teste comuns
        _answerOption = new AnswerOption
        {
            Id = Guid.NewGuid(),
            Question = _question,
            QuestionId = _question.Id,
            IsCorrectOption = true
        };

        _quiz = new Quiz
        {
            ExpiresInSeconds = 120,
            Questions = [_question]
        };

        _match = new Match
        {
            Id = Guid.NewGuid(),
            Status = EMatchStatus.Created,
            Quiz = _quiz,
            UserId = _userId,
            CreatedAt = DateTime.UtcNow,
            Responses = []
        };

        _response = new Response
        {
            Id = Guid.NewGuid(),
            Match = _match,
            AnswerOption = _answerOption,
            AnswerOptionId = _answerOption.Id,
            IsCorrect = true
        };
    }

    [Fact]
    public async Task Execute_ValidInput_ReturnsResult()
    {
        // Arrange
        ConfigureCommonMocks();

        // Act
        var result = await _useCase.Execute(_userId, _match.Id, _answerOption.Id);

        // Assert
        _responseServiceMock.Verify(x => x.AddAsync(It.IsAny<Response>()), Times.Once);
        _matchServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Match>()), Times.AtLeastOnce());
    }

    [Fact]
    public async Task Execute_AlreadyExpired_ThrowsInvalidOperationException()
    {
        // Arrange
        ConfigureCommonMocks();
        _quiz.Expires = true; // Simula expiração
        
        _matchServiceMock.Setup(x => x.AlreadyMatchExpired(It.IsAny<Match>())).Returns(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Execute(_userId, _match.Id, _answerOption.Id));
    }

    [Fact]
    public async Task Execute_AlreadyFinished_ThrowsInvalidOperationException()
    {
        // Arrange
        ConfigureCommonMocks();
        _match.Status = EMatchStatus.Finished;

        _matchServiceMock.Setup(x => x.EnsureNotCompleted(It.IsAny<Match>())).Throws<InvalidOperationException>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Execute(_userId, _match.Id, _answerOption.Id));

        _responseServiceMock.Verify(x => x.AddAsync(It.IsAny<Response>()), Times.Never);
    }

    [Fact]
    public async Task Execute_AlreadyFailed_ThrowsInvalidOperationException()
    {
        // Arrange
        ConfigureCommonMocks();
        
        _matchServiceMock.Setup(x => x.EnsureNotCompleted(It.IsAny<Match>())).Throws<InvalidOperationException>();
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Execute(_userId, _match.Id, _answerOption.Id));

        _responseServiceMock.Verify(x => x.AddAsync(It.IsAny<Response>()), Times.Never);
    }

    [Fact]
    public async Task Execute_UserDoesNotHaveAccess_ThrowsUnauthorizedException()
    {
        // Arrange
        ConfigureCommonMocks();
        _authServiceMock
            .Setup(x => x.ValidateSameUser(It.IsAny<Guid>(), _userId))
            .Throws<UnauthorizedAccessException>();

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _useCase.Execute(_userId, _match.Id, _answerOption.Id));

        _responseServiceMock.Verify(x => x.AddAsync(It.IsAny<Response>()), Times.Never);
    }

    [Fact]
    public async Task Execute_LastQuestion_MustFinish_ReturnsResult()
    {
        // Arrange
        ConfigureCommonMocks();
        _quiz.Questions = [_question]; // Apenas uma pergunta na lista
        
        // Act
        var result = await _useCase.Execute(_userId, _match.Id, _answerOption.Id);

        // Assert
        _responseServiceMock.Verify(x => x.AddAsync(It.IsAny<Response>()), Times.Once);
        _matchServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Match>()), Times.AtLeastOnce);
        _matchServiceMock.Verify(x => x.FinalizeMatch(It.IsAny<Match>()), Times.Once);
    }

    private void ConfigureCommonMocks()
    {
        _responseServiceMock.Setup(x => x.CreateResponse(It.IsAny<Match>(), It.IsAny<AnswerOption>())).Returns(_response);
        
        _answerOptionServiceMock
            .Setup(x => x.GetByIdAsync(_answerOption.Id))
            .ReturnsAsync(_answerOption);

        _matchServiceMock
            .Setup(x => x.GetByIdAsync(_match.Id))
            .ReturnsAsync(_match);

        _authServiceMock
            .Setup(x => x.ValidateSameUser(_match.UserId, _userId))
            .Verifiable();
    }
}
