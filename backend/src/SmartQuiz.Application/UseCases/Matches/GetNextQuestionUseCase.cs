using AutoMapper;
using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Matches;

public class GetNextQuestionUseCase
{
    private readonly IMatchService _matchService;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    
    public GetNextQuestionUseCase(IMatchService matchService, IMapper mapper, IAuthService authService)
    {
        _matchService = matchService;
        _mapper = mapper;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(Guid matchId, Guid userId)
    {
        //Busca a partida
        var match = await _matchService.GetByIdAsync(matchId);
        if (match is null) 
            throw new ArgumentException("Partida não encontrada");

        _authService.ValidateSameUser(match.UserId, userId);

        //Verifica se já foi finalizada
        _matchService.EnsureNotCompleted(match);

        //Busca próxima questão
        var nextQuestion = await _matchService.GetNextQuestion(match);
        
        var dto = new
        {
            IsLastQuestion = nextQuestion.Order + 1 == match.Quiz.Questions.Count,
            Question = _mapper.Map<GetQuestionDto>(nextQuestion)
        };

        return new ResultDto(dto);
    }
}