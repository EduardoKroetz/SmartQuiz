using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace QuizDev.API.Extensions;

public static class ModelStateExtension
{
    public static List<string> GetErrors(this ModelStateDictionary modelState)
    {
        var errors = modelState.Select(x => 
            x.Value.Errors.Select(x => x.ErrorMessage).ToList())
        .FirstOrDefault();

        return errors;
    }
}
