
using QuizDev.Application.Exceptions;
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Questions;

public class GetQuestionDetailsUseCase
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionDetailsUseCase(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<ResultDto> Execute(Guid questionId)
    {
        var questionDetails = await _questionRepository.GetQuestionDetails(questionId);
        if (questionDetails == null)
        {
            throw new NotFoundException("Questão não encontrada");
        }

        return new ResultDto(questionDetails);
    }

}

