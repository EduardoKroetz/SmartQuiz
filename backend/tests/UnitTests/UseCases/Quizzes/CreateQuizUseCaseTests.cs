using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.UseCases.Quizzes;
using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Quizzes;

public class CreateQuizUseCaseTests
{
    private readonly Mock<IQuizService> _quizServiceMock;
    private readonly CreateQuizUseCase _createQuizUseCase;

    public CreateQuizUseCaseTests()
    {
        _quizServiceMock = new Mock<IQuizService>();
        _createQuizUseCase = new CreateQuizUseCase(_quizServiceMock.Object);
    }

    [Fact]
    public async Task Execute_ValidQuiz_ShouldCreate()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var quiz = new Quiz { UserId = userId };
        var quizDto = new EditorQuizDto { Title = "title", Description = "description", Expires = false, ExpiresInSeconds = 0, Difficulty = "medium", Theme = "quiz"};

        _quizServiceMock.Setup(x => x.CreateQuiz(It.IsAny<EditorQuizDto>(), userId)).Returns(quiz);
        
        //Act
        await _createQuizUseCase.Execute(quizDto, userId);

        //Assert
        _quizServiceMock.Verify(x => x.AddAsync(It.IsAny<Quiz>()), Times.Once);
    }
}
