using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.DTOs.Matches;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.Services;

public class ResponseService : IResponseService
{
    private readonly IResponseRepository _responseRepository;
    private readonly IMapper _mapper;

    public ResponseService(IResponseRepository responseRepository, IMapper mapper)
    {
        _responseRepository = responseRepository;
        _mapper = mapper;
    }

    public async Task AddAsync(Response response)
    {
       await _responseRepository.AddAsync(response);
    }

    public async Task<IEnumerable<GetResponseDto>> GetResponsesByMatch(Match match)
    {
       return await _responseRepository.Query()
            .Include(x => x.AnswerOption)
            .ThenInclude(x => x.Question)
            .ThenInclude(x => x.AnswerOptions)
            .Where(x => x.MatchId.Equals(match.Id))
            .Select(x => new GetResponseDto
            {
                QuestionId = x.AnswerOption.Id,
                QuestionOrder = x.AnswerOption.Question.Order,
                AnswerOptionId = x.AnswerOptionId,
                AnswerOption = _mapper.Map<GetAnswerOptionDto>(x.AnswerOption),
                CorrectOption = _mapper.Map<GetAnswerOptionDto>(x.AnswerOption.Question.AnswerOptions.FirstOrDefault(x => x.IsCorrectOption == true)),
                IsCorrect = x.IsCorrect
            })
            .OrderBy(x => x.QuestionOrder)
            .ToListAsync();
    }

    public Response CreateResponse(Match match, AnswerOption option)
    {
        //Verificar se a questão da opção de resposta está entre as questões do quiz
        if (match.Quiz.Questions.Any(x => x.Id.Equals(option.QuestionId)) == false)
        {
            throw new InvalidOperationException("A opção de resposta não está disponível");
        }

        var response = new Response
        {
            Id = Guid.NewGuid(),
            MatchId = match.Id,
            IsCorrect = option.IsCorrectOption,
            AnswerOptionId = option.Id
        };

        match.Responses.Add(response);

        return response;
    }

}