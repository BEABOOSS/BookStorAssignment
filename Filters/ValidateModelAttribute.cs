using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookStore.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                Console.WriteLine("Well that failed");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Method: " + context.HttpContext.Request.Method.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
