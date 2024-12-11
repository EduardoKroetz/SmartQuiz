﻿using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.UseCases.Quizzes;
using SmartQuiz.Core.DTOs.Quizzes;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Quizzes;

public class CreateQuizUseCaseTests
{
    private readonly Mock<IQuizRepository> _quizRepositoryMock;
    private readonly CreateQuizUseCase _createQuizUseCase;

    public CreateQuizUseCaseTests()
    {
        _quizRepositoryMock = new Mock<IQuizRepository>();
        _createQuizUseCase = new CreateQuizUseCase(_quizRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_ValidQuiz_ShouldCreate()
    {
        //Arrange
        var quizDto = new EditorQuizDto { Title = "tite", Description = "description", };

        //Act
        var result = await _createQuizUseCase.Execute(quizDto, Guid.NewGuid());

        //Assert
        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        _quizRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Quiz>()), Times.Once);
    }
}