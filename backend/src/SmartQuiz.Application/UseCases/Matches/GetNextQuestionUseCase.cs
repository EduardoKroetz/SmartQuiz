
using SmartQuiz.Core.DTOs.AnswerOptions;
using SmartQuiz.Core.DTOs.Questions;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Matches;

public class GetNextQuestionUseCase
{
    private readonly IMatchRepository _matchRepository;
    private readonly IQuestionRepository _questionRepository;

    public GetNextQuestionUseCase(IMatchRepository matchRepository, IQuestionRepository questionRepository)
    {
        _matchRepository = matchRepository;
        _questionRepository = questionRepository;
    }

    public async Task<ResultDto> Execute(Guid matchId, Guid userId)
    {
        //Busca a partida
        var match = await _matchRepository.GetAsync(matchId);
        if (match == null)
        {
            throw new ArgumentException("Partida não encontrada");
        }

        if (userId != match.UserId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");
        }

        //Verifica se já foi finalizada
        if (match.Status == Core.Enums.EMatchStatus.Finished)
        {
            throw new InvalidOperationException("Essa partida já foi finalizada");
        }

        if (match.Status == Core.Enums.EMatchStatus.Failed)
        {
            throw new InvalidOperationException("Partida expirada");

        }

        //Busca próxima questão
        var nextQuestion = await _matchRepository.GetNextQuestion(match);

        if (nextQuestion == null)
        {
            if (match.Responses.Count == 0)
            {
                nextQuestion = await _questionRepository.GetQuizQuestionByOrder(match.QuizId, 0);

                if (nextQuestion == null)
                {
                    throw new ArgumentException("Não foi possível buscar a primeira questão do Quiz");
                }

            }
            else
            {
                return new ResultDto(null);
            }
        }

        var dto = new
        {
            IsLastQuestion = nextQuestion.Order + 1 == match.Quiz.Questions.Count,
            Question = new GetQuestionDto
            {
                Id = nextQuestion.Id,
                Text = nextQuestion.Text,
                QuizId = nextQuestion.QuizId,
                Order = nextQuestion.Order,
                Options = nextQuestion.AnswerOptions.Select(o => new GetAnswerOptionDto(o.Id, o.Response)).ToList()
            }
        };

        return new ResultDto(dto);
    }
}
