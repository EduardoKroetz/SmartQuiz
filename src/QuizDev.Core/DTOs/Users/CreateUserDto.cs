using System.ComponentModel.DataAnnotations;

namespace QuizDev.Core.DTOs.Users;

public class CreateUserDto
{
    [Required(ErrorMessage = "Informe o nome de usuário")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Informe o e-mail")]
    [EmailAddress(ErrorMessage = "Informe o e-mail em um formato válido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Informe a senha")]
    [MinLength(6, ErrorMessage = "A senha deve possuir no mínimo 6 caracteres")]
    [MaxLength(18, ErrorMessage = "A senha deve possuir no máximo 18 caracteres")]
    public string Password { get; set; }
}
