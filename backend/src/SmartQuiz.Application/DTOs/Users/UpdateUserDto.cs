using System.ComponentModel.DataAnnotations;

namespace SmartQuiz.Application.DTOs.Users;

public class UpdateUserDto
{
    [Required(ErrorMessage = "Informe o nome de usuário")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Informe o e-mail")]
    [EmailAddress(ErrorMessage = "Informe o e-mail em um formato válido")]
    public string Email { get; set; }
}