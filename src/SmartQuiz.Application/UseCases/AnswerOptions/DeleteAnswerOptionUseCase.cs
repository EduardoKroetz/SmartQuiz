
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.AnswerOptions;

public class DeleteAnswerOptionUseCase
{
    private readonly IAnswerOptionRepository _answerOptionRepository;
    private readonly IQuestionRepository _questionRepository;

    public DeleteAnswerOptionUseCase(IAnswerOptionRepository answerOptionRepository, IQuestionRepository questionRepository)
    {
        _answerOptionRepository = answerOptionRepository;
        _questionRepository = questionRepository;
    }

    public async Task<ResultDto> Execute(Guid answerOptionId, Guid userId)
    {
        var answerOption = await _answerOptionRepository.GetById(answerOptionId);
        if (answerOption == null)
        {
            throw new NotFoundException("Opção de resposta não encontrada");
        }

        var question = await _questionRepository.GetAsync(answerOption.QuestionId);
        if (question == null)
        {
            throw new NotFoundException("Não foi possível encontrar a questão relacionada a opção de resposta");
        }

        if (question.Quiz.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");
        }

        if (question.Options.Count <= 2)
        {
            throw new InvalidOperationException("Não é possível deletar a opção de resposta pois a Questão relacionada deve possuir no mínimo duas opções de resposta");
        }

        await _answerOptionRepository.DeleteAsync(answerOption);

        return new ResultDto(new { AnswerOptionId = answerOption.Id, QuestionId = question.Id });
    }
}
