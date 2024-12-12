using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SmartQuiz.API.Extensions;

public static class ModelStateExtension
{
    public static List<string> GetErrors(this ModelStateDictionary modelState)
    {
        var errors = modelState
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage)
            .ToList();

        return errors;
    }
}
