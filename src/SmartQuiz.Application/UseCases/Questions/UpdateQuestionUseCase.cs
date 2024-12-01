
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Core.DTOs.Questions;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Questions;

public class UpdateQuestionUseCase
{
    private readonly IQuestionRepository _questionRepository;

    public UpdateQuestionUseCase(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<ResultDto> Execute(Guid questionId, UpdateQuestionDto dto, Guid userId)
    {
        var question = await _questionRepository.GetAsync(questionId);
        if (question == null)
        {
            throw new NotFoundException("Questão não encontrada");
        }

        if (question.Quiz.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");
        }

        question.Text = dto.Text;

        await _questionRepository.UpdateAsync(question);

        return new ResultDto(new { question.Id, question.Text });
    }
}
