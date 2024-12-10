using System.ComponentModel.DataAnnotations;

namespace SmartQuiz.Core.DTOs.Users;

public class UpdatePasswordDto
{
    [Required(ErrorMessage = "Informe a senha")]
    [MinLength(6, ErrorMessage = "A senha deve possuir no mínimo 6 caracteres")]
    [MaxLength(18, ErrorMessage = "A senha deve possuir no máximo 18 caracteres")]
    public string CurrentPassword { get; set; }
    
    [Required(ErrorMessage = "Informe a senha")]
    [MinLength(6, ErrorMessage = "A senha deve possuir no mínimo 6 caracteres")]
    [MaxLength(18, ErrorMessage = "A senha deve possuir no máximo 18 caracteres")]
    public string NewPassword { get; set; }
}