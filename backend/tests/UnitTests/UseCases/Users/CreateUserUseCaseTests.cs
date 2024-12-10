using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.UseCases.Users;
using SmartQuiz.Core.DTOs.Users;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Users;

public class CreateUserUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly CreateUserUseCase _createUserUseCase;

    public CreateUserUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _authServiceMock = new Mock<IAuthService>();
        _createUserUseCase = new CreateUserUseCase(_userRepositoryMock.Object, _authServiceMock.Object);
    }

    [Fact]
    public async Task Execute_ValidUser_CreatesUserAndReturnsToken()
    {
        // Arrange
        var createUserDto = new CreateUserDto { Username = "Test", Email = "test@example.com", Password = "password" };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(createUserDto.Email)).ReturnsAsync((User)null);
        _authServiceMock.Setup(a => a.HashPassword(createUserDto.Password)).Returns("hashed_password");
        _authServiceMock.Setup(a => a.GenerateJwtToken(It.IsAny<User>())).Returns("mocked_token");

        // Act
        var result = await _createUserUseCase.Execute(createUserDto);


        // Assert
        _userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);

        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        Assert.Equal("mocked_token", (string)data.Token);
    }
}
