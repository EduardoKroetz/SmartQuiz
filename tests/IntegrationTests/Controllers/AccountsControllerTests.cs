
using System.Net.Http.Headers;
using System.Net.Http.Json;
using IntegrationTests.Factories;
using Newtonsoft.Json;
using SmartQuiz.Core.DTOs.Users;

namespace IntegrationTests.Controllers.Users;

public class AccountsControllerTests : IClassFixture<SmartQuizWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly SmartQuizWebApplicationFactory _factory;

    public AccountsControllerTests(SmartQuizWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task LoginUser_ShouldReturnOk()
    {
        //Arrange
        var data = await Utils.CreateUserAsync(_client, "test", "test@gmail.com");
        var userId = (Guid)data.id;

        var loginDto = new LoginUserDto { Email = "test@gmail.com", Password = "password" };

        //Act
        var response = await _client.PostAsJsonAsync("api/accounts/login", loginDto);

        //Assert
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(content.data.token);
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnOk()
    {
        //Arrange
        var createUserDto = new CreateUserDto { Username = "user", Email = "user@gmail.com", Password = "password" };

        //Act
        var response = await _client.PostAsJsonAsync("api/accounts/register", createUserDto);

        //Assert
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(content.data.token);
    }

    [Fact]
    public async Task GetDetailsAsync_ShouldReturnOk()
    {
        //Arrange
        var data = await Utils.CreateUserAsync(_client, "userTest", "userTest@gmail.com");
        var userId = (Guid)data.id;

        //Act
        var response = await _client.GetAsync($"api/accounts/{userId}");

        //Assert
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        Assert.Equal(userId, (Guid)content.data.id);
    }

    [Fact]
    public async Task GetAuthenticatedUserDetailsAsync_ShouldReturnOk()
    {
        //Arrange
        var data = await Utils.CreateUserAsync(_client, "authUser", "authUser@gmail.com");
        var userId = (Guid)data.id;
        var token = (string)data.token;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //Act
        var response = await _client.GetAsync($"api/accounts");

        //Assert
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        Assert.Equal(userId, (Guid)content.data.id);
        _client.DefaultRequestHeaders.Authorization = null;
    }

    [Fact]
    public async Task GetUserMatchesAsync_ShouldReturnOk()
    {
        //Arrange
        var data = await Utils.CreateUserAsync(_client, "matches", "matches@gmail.com");
        var userId = (Guid)data.id;

        //Act
        var response = await _client.GetAsync($"api/accounts/{userId}/matches");

        //Assert
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(content.data);
    }

    [Fact]
    public async Task GetUserQuizzesAsync_ShouldReturnOk()
    {
        //Arrange
        var data = await Utils.CreateUserAsync(_client, "quizzes", "quizzes@gmail.com");
        var userId = (Guid)data.id;

        //Act
        var response = await _client.GetAsync($"api/accounts/{userId}/quizzes");

        //Assert
        response.EnsureSuccessStatusCode();
        var content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(content.data);
    }
}
