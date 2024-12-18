namespace SmartQuiz.Application.DTOs.Matches;

public class GetMatchesDto
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string? Reference { get; set; }
    public string? Status { get; set; }
    public bool? Reviewed { get; set; }
    public string? OrderBy { get; set; }
}
