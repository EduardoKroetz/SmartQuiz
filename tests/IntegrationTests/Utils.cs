using Newtonsoft.Json;
using QuizDev.Core.DTOs.Users;
using System.Net.Http.Json;

namespace IntegrationTests;

public static class Utils
{
    public static async Task<dynamic> CreateUserAsync(HttpClient client ,string username, string email, string password = "password")
    {
        var createUserDto = new CreateUserDto { Username = username, Email = email, Password = password };
        var createUserResponse = await client.PostAsJsonAsync("api/accounts/register", createUserDto); //Criar novo usuário
        var userContent = JsonConvert.DeserializeObject<dynamic>(await createUserResponse.Content.ReadAsStringAsync());
        return userContent.data;
    }
}
