using BookStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookStore.Filters
{
    public class EnsureBookExistAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var service = (BookStoreContext)context.HttpContext.RequestServices
                .GetService(typeof(Data.BookStoreContext));
            int bookId = (int)context.ActionArguments["id"];

            if (!service.DoesBookExist(bookId))
            {
                context.Result = new NotFoundResult();
            }
        }
    }
}
