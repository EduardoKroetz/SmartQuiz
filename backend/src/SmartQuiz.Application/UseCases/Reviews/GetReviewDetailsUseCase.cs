using AutoMapper;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Reviews;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Reviews;

public class GetReviewDetailsUseCase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    
    public GetReviewDetailsUseCase(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public async Task<ResultDto> Execute(Guid reviewId)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId);
        if (review == null) 
            throw new NotFoundException("Avaliação não encontrada");

        var reviewDto = _mapper.Map<GetReviewDto>(review);
        
        return new ResultDto(reviewDto);
    }
}