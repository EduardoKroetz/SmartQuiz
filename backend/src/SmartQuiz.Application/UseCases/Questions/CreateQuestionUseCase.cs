using SmartQuiz.Application.Exceptions;
using SmartQuiz.Core.DTOs.Questions;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Questions;

public class CreateQuestionUseCase
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IAnswerOptionRepository _answerOptionRepository;

    public CreateQuestionUseCase(IQuestionRepository questionRepository, IQuizRepository quizRepository, IAnswerOptionRepository answerOptionRepository)
    {
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
        _answerOptionRepository = answerOptionRepository;
    }

    public async Task<ResultDto> Execute(CreateQuestionDto createQuestionDto, Guid userId)
    {
        var quiz = await _quizRepository.GetAsync(createQuestionDto.QuizId, true);
        if (quiz == null)
        {
            throw new NotFoundException("Quiz não encontrado");
        }

        if (quiz.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");
        }

        //Valida se possui opção correta
        var correctOptionsCount = createQuestionDto.Options.Count(x => x.IsCorrectOption);
        if (correctOptionsCount == 0)
        {
            throw new ArgumentException("Informe uma opção de resposta correta");
        }

        //Valida se possui somente 1 opção correta
        if (correctOptionsCount > 1)
        {
            throw new ArgumentException("Só deve possuir uma opção correta da questão");
        }

        //Verifica se já possui alguma questão nessa ordem (Question.Order)
        if (quiz.VerifyExistsOrder(createQuestionDto.Order))
        {
            //Se já possui, atualiza todas as posições
            var questions = quiz.GetQuestionsByOrderGratherThan(createQuestionDto.Order - 1);
            questions.ForEach(x =>
            {
                x.Order++;
            });

            await _questionRepository.UpdateRangeAsync(questions);
        }

        //Criar nova questão
        var question = new Question
        {
            Id = Guid.NewGuid(),
            Text = createQuestionDto.Text,
            QuizId = createQuestionDto.QuizId,
            Order = createQuestionDto.Order
        };

        quiz.Questions.Add(question);

        if (quiz.VerifyQuestionsSequenceOrder() == false)
        {
            throw new ArgumentException($"A sequência da ordem das questões do Quiz ficou inválida, verifique qual a ordem das questões e tente novamente");
        }

        await _questionRepository.CreateAsync(question);

        foreach (var answerOption in createQuestionDto.Options)
        {
            var newOption = new AnswerOption
            {
                Id = Guid.NewGuid(),
                IsCorrectOption = answerOption.IsCorrectOption,
                QuestionId = question.Id,
                Response = answerOption.Response,
            };

            await _answerOptionRepository.CreateAsync(newOption);
        }

        return new ResultDto(new { question.Id });
    }

}
