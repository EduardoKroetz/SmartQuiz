
namespace QuizDev.Application.DTOs.Responses;

public class ResultDto
{
    public ResultDto(object? data)
    {
        Success = true;
        Data = data;
        Errors = null;
    }

    public ResultDto(List<string>? errors)
    {
        Success = false;
        Data = null;
        Errors = errors;
    }

    public bool Success { get; set; }
    public object? Data { get; set; }
    public List<string>? Errors { get; set; }


}
