using QuizDev.Application.DTOs.Questions;
using QuizDev.Application.DTOs.Responses;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Questions;

public class CreateQuestionUseCase
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionOptionRepository _questionOptionRepository;

    public CreateQuestionUseCase(IQuestionRepository questionRepository, IQuizRepository quizRepository, IQuestionOptionRepository questionOptionRepository)
    {
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
        _questionOptionRepository = questionOptionRepository;
    }

    public async Task<ResultDto> Execute(CreateQuestionDto createQuestionDto)
    {
        var quiz = await _quizRepository.GetAsync(createQuestionDto.QuizId);
        if (quiz == null)
        {
            throw new ArgumentException("Quiz não encontrado");
        }

        //Valida se possui opção correta
        var correctOptionsCount = createQuestionDto.CreateOptionsDtos.Count(x => x.IsCorrectOption);
        if(correctOptionsCount == 0)
        {
            throw new ArgumentException("Informe uma opção de resposta correta");
        }

        //Valida se possui somente 1 opção correta
        if (correctOptionsCount > 1)
        {
            throw new ArgumentException("Só deve possuir uma opção correta da questão");
        }

        //Criar nova questão
        var question = new Question
        {
            Id = Guid.NewGuid(),
            Text = createQuestionDto.Text,
            QuizId = createQuestionDto.QuizId
        };

        await _questionRepository.CreateAsync(question);

        foreach (var questionOption in createQuestionDto.CreateOptionsDtos)
        {
            var newOption = new QuestionOption
            {
                Id = Guid.NewGuid(),
                IsCorrectOption = questionOption.IsCorrectOption,
                QuestionId = question.Id,
                Response = questionOption.Response
            };

            await _questionOptionRepository.CreateAsync(newOption);
        }

        return new ResultDto(new { question.Id });
    }

}
