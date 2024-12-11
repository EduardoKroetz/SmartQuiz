namespace SmartQuiz.Application.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToBrazilianTime(this DateTime dateTime)
    {
        var brazilianTime = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        return TimeZoneInfo.ConvertTime(dateTime, brazilianTime);
    }
}