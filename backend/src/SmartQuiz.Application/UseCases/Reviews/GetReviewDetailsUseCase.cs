using AutoMapper;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Reviews;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Reviews;

public class GetReviewDetailsUseCase
{
    private readonly IReviewService _reviewService;
    private readonly IMapper _mapper;

    public GetReviewDetailsUseCase(IReviewService reviewService, IMapper mapper)
    {
        _reviewService = reviewService;
        _mapper = mapper;
    }

    public async Task<ResultDto> Execute(Guid reviewId)
    {
        var review = await _reviewService.GetByIdAsync(reviewId);
        if (review == null) 
            throw new NotFoundException("Avaliação não encontrada");

        var reviewDto = _mapper.Map<GetReviewDto>(review);
        
        return new ResultDto(reviewDto);
    }
}