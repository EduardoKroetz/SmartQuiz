using System.ComponentModel.DataAnnotations;
using SmartQuiz.Application.DTOs.AnswerOptions;

namespace SmartQuiz.Application.DTOs.Questions;

public class CreateQuestionDto
{
    [Required(ErrorMessage = "Informe a pergunta")]
    public string Text { get; set; }

    [Required(ErrorMessage = "Informe o Quiz relacionado")]
    public Guid QuizId { get; set; }

    [Required(ErrorMessage = "Informe as opções de resposta da questão")]
    [MinLength(2, ErrorMessage = "Informe pelo menos duas opções de resposta para a questão")]
    public List<CreateAnswerOptionInQuestionDto> Options { get; set; }

    [Required(ErrorMessage = "Informe a ordem dessa questão no Quiz")]
    public int Order { get; set; }
}