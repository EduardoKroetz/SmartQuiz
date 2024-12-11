namespace SmartQuiz.Application.DTOs.Results;

public class PaginatedResultDto
{
    public PaginatedResultDto(int pageSize, int pageNumber, int total, int nextPage, object? data)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        Total = total;
        NextPage = nextPage;
        Data = data;
        Success = true;
    }

    public PaginatedResultDto(int pageSize, int pageNumber, List<string>? errors)
    {
        Success = false;
        PageSize = pageSize;
        PageNumber = pageNumber;
        Total = 0;
        NextPage = pageNumber;
        Data = null;
        Errors = errors;
    }

    public bool Success { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int Total { get; set; }
    public int NextPage { get; set; }
    public object? Data { get; set; }
    public List<string>? Errors { get; set; }
}