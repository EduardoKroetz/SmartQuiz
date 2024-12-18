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

    public void Validate()
    {
        //Valida se possui opção correta
        var correctOptionsCount = Options.Count(x => x.IsCorrectOption);
        if (correctOptionsCount == 0) 
            throw new ArgumentException("Informe uma opção de resposta correta");

        //Valida se possui somente 1 opção correta
        if (correctOptionsCount > 1) 
            throw new ArgumentException("Só deve possuir uma opção correta da questão");
    }
}