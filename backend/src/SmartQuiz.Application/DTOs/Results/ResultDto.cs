namespace SmartQuiz.Application.DTOs.Responses;

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

public class ResultDto<T> where T : class
{
    public ResultDto(T? data)
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
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
}