
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuizDev.API.Extensions;
using QuizDev.Core.DTOs.Responses;

namespace QuizDev.API.Filters;

public class ValidateModelStateFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var resultDto = new ResultDto(context.ModelState.GetErrors());
            context.Result = new BadRequestObjectResult(resultDto);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}
