using AutoMapper;
using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.DTOs.AutoMapper;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.UseCases.Matches;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;
using Match = SmartQuiz.Core.Entities.Match;

namespace UnitTests.UseCases.Matches;

public class GetNextQuestionUseCaseTests
{
    private readonly Mock<IMatchService> _matchServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly IMapper _mapper;
    private readonly GetNextQuestionUseCase _useCase;

    public GetNextQuestionUseCaseTests()
    {
        _matchServiceMock = new Mock<IMatchService>();
        _authServiceMock = new Mock<IAuthService>();
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }).CreateMapper();
        _useCase = new GetNextQuestionUseCase(_matchServiceMock.Object, _mapper, _authServiceMock.Object);
    }

    [Fact]
    public async Task Execute_ValidInput_ReturnsResult()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var nextQuestion = new Question { Id = Guid.NewGuid(), Order = 0, Text = "", AnswerOptions = [], QuizId = Guid.NewGuid() };
        var quiz = new Quiz { Questions = new List<Question>() { nextQuestion } };
        var match = new SmartQuiz.Core.Entities.Match { Id = Guid.NewGuid(), Quiz = quiz, UserId = userId, Status = SmartQuiz.Core.Enums.EMatchStatus.Created };

        _matchServiceMock.Setup(x => x.GetByIdAsync(match.Id)).ReturnsAsync(match);
        _matchServiceMock.Setup(x => x.GetNextQuestion(match)).ReturnsAsync(nextQuestion);

        //Act
        var result = await _useCase.Execute(match.Id, userId);

        //Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        Assert.True((bool)data.IsLastQuestion);
        Assert.Equal(SmartQuiz.Core.Enums.EMatchStatus.Created, match.Status);
    }

    [Fact]
    public async Task Execute_NoNextQuestion_AndResponseCountEqual0_ReturnsFirstQuestion()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var nextQuestion = new Question { Id = Guid.NewGuid(), Order = 0, Text = "", AnswerOptions = [], QuizId = Guid.NewGuid() };
        var quiz = new Quiz { Questions = new List<Question>() { new(), new() } };
        var match = new SmartQuiz.Core.Entities.Match { Id = Guid.NewGuid(), Quiz = quiz, UserId = userId, Status = SmartQuiz.Core.Enums.EMatchStatus.Created, Responses = new List<Response>() };

        _matchServiceMock.Setup(x => x.GetByIdAsync(match.Id)).ReturnsAsync(match);
        _matchServiceMock.Setup(x => x.GetNextQuestion(match)).ReturnsAsync(nextQuestion);
        
        //Act
        await _useCase.Execute(match.Id, userId);

        //Assert
        Assert.Equal(SmartQuiz.Core.Enums.EMatchStatus.Created, match.Status);
    }

    [Fact]
    public async Task Execute_MatchFinished_ThrowsInvalidOperationException()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var nextQuestion = new Question { Id = Guid.NewGuid(), Order = 0, Text = "", AnswerOptions = [], QuizId = Guid.NewGuid() };
        var match = new Match { UserId = userId, Status = SmartQuiz.Core.Enums.EMatchStatus.Finished };

        _matchServiceMock.Setup(x => x.EnsureNotCompleted(It.IsAny<Match>())).Throws<InvalidOperationException>();
        _matchServiceMock.Setup(x => x.GetByIdAsync(match.Id)).ReturnsAsync(match);
        _matchServiceMock.Setup(x => x.GetNextQuestion(match)).ReturnsAsync(nextQuestion);

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Execute(match.Id, userId));
    }

    [Fact]
    public async Task Execute_MatchFailed_ThrowsInvalidOperationException()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var nextQuestion = new Question { Id = Guid.NewGuid(), Order = 0, Text = "", AnswerOptions = [], QuizId = Guid.NewGuid() };
        var match = new Match { UserId = userId, Status = SmartQuiz.Core.Enums.EMatchStatus.Failed };

        _matchServiceMock.Setup(x => x.EnsureNotCompleted(It.IsAny<Match>())).Throws<InvalidOperationException>();
        _matchServiceMock.Setup(x => x.GetByIdAsync(match.Id)).ReturnsAsync(match);
        _matchServiceMock.Setup(x => x.GetNextQuestion(match)).ReturnsAsync(nextQuestion);

        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Execute(match.Id, userId));
    }

    [Fact]
    public async Task Execute_ItsNotWhoCreatedTheMatch_ThrowsUnauthorizedAccessException()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var nextQuestion = new Question { Id = Guid.NewGuid(), Order = 0, Text = "", AnswerOptions = [], QuizId = Guid.NewGuid() };
        var match = new SmartQuiz.Core.Entities.Match { UserId = Guid.NewGuid(), Status = SmartQuiz.Core.Enums.EMatchStatus.Failed };

        _authServiceMock.Setup(x => x.ValidateSameUser(It.IsAny<Guid>(), userId)).Throws<UnauthorizedAccessException>();
        _matchServiceMock.Setup(x => x.GetByIdAsync(match.Id)).ReturnsAsync(match);
        _matchServiceMock.Setup(x => x.GetNextQuestion(match)).ReturnsAsync(nextQuestion);

        //Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _useCase.Execute(match.Id, userId));
    }
}
