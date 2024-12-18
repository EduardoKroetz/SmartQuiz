using Moq;
using Newtonsoft.Json;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.UseCases.Users;
using SmartQuiz.Application.DTOs.Users;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace UnitTests.UseCases.Users;

public class CreateUserUseCaseTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly CreateUserUseCase _createUserUseCase;

    public CreateUserUseCaseTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _authServiceMock = new Mock<IAuthService>();
        _createUserUseCase = new CreateUserUseCase(_authServiceMock.Object, _userServiceMock.Object);
    }

    [Fact]
    public async Task Execute_ValidUser_CreatesUserAndReturnsToken()
    {
        // Arrange
        var createUserDto = new CreateUserDto { Username = "Test", Email = "test@example.com", Password = "password" };
        var user = new User { Email = createUserDto.Email, Username = createUserDto.Username };
        _userServiceMock.Setup(r =>
            r.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), false)).Returns(user);
        _userServiceMock.Setup(r => r.GetByEmailAsync(createUserDto.Email)).ReturnsAsync((User)null);
        _authServiceMock.Setup(a => a.HashPassword(createUserDto.Password)).Returns("hashed_password");
        _authServiceMock.Setup(a => a.GenerateJwtToken(It.IsAny<User>())).Returns("mocked_token");

        // Act
        var result = await _createUserUseCase.Execute(createUserDto);


        // Assert
        _userServiceMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);

        var data = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Data));
        Assert.Equal("mocked_token", (string)data.Token);
    }
}
