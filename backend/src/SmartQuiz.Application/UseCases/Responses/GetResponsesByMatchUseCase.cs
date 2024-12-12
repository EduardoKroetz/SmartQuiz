using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Responses;

public class GetResponsesByMatchUseCase
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMapper _mapper;
    private readonly IResponseRepository _responseRepository;

    public GetResponsesByMatchUseCase(IMatchRepository matchRepository, IMapper mapper, IResponseRepository responseRepository)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
        _responseRepository = responseRepository;
    }
    
    public async Task<ResultDto> Execute(Guid matchId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null) 
            throw new NotFoundException("Partida não encontrada");

        var responses  = await _responseRepository.Query()
            .Include(x => x.AnswerOption)
            .ThenInclude(x => x.Question)
            .ThenInclude(x => x.AnswerOptions)
            .Where(x => x.MatchId.Equals(matchId))
            .Select(x => new GetResponseDto
            {
                QuestionId = x.AnswerOption.Id,
                QuestionOrder = x.AnswerOption.Question.Order,
                AnswerOptionId = x.AnswerOptionId,
                AnswerOption = _mapper.Map<GetAnswerOptionDto>(x.AnswerOption),
                CorrectOption = _mapper.Map<GetAnswerOptionDto>(x.AnswerOption.Question.AnswerOptions.FirstOrDefault(x => x.IsCorrectOption == true)),
                IsCorrect = x.IsCorrect
            })
            .ToListAsync();
        
        var ordenedResponses = responses.OrderBy(x => x.QuestionOrder).ToList();
        
        return new ResultDto(ordenedResponses);
    }
}