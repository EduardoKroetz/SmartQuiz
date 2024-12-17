using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Application.Services.Interfaces;

public interface IResponseService
{
    Task AddAsync(Response response);
    Task<IEnumerable<GetResponseDto>> GetResponsesByMatch(Match match);
    
    Response CreateResponse(Match match, AnswerOption option);
    
}