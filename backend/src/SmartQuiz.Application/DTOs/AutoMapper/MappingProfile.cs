using AutoMapper;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.DTOs.Matches;
using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Reviews;
using SmartQuiz.Application.DTOs.Users;
using SmartQuiz.Application.Extensions;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;

namespace SmartQuiz.Application.DTOs.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AnswerOption, GetAnswerOptionDto>()
            .ConstructUsing(src => new GetAnswerOptionDto(src.Id, src.Response));
        CreateMap<CreateAnswerOptionDto, AnswerOption>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        CreateMap<Match, GetMatchDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt.ToBrazilianTime()))
            .ForMember(dest => dest.RemainingTimeInSeconds, opt => opt.MapFrom(x => x.GetRemainingTime()));
        
        CreateMap<Question, GetQuestionDto>()
            .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.AnswerOptions));
        
        CreateMap<Quiz, GetQuizDto>()
            .ConstructUsing(src => new GetQuizDto(src.Id, src.Title, src.Description, src.Expires, src.ExpiresInSeconds, src.IsActive, src.Theme, src.Questions.Count, src.Difficulty, src.UserId));
        
        CreateMap<Review, GetReviewDto>();
    }
}