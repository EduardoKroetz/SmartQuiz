
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartQuiz.API.Extensions;
using SmartQuiz.Core.DTOs.Responses;

namespace SmartQuiz.API.Filters;

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
