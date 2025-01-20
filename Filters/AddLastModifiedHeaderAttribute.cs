using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookStore.Filters
{
    public class AddLastModifiedHeaderAttribute : ResultFilterAttribute
    {
        // Set the book model last modified date to the response object last modified time
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult result &&
                result.Value is Book book)
            {
                var bookModelDate = book.ModifiedDate;
                context.HttpContext.Response
                    .GetTypedHeaders().LastModified = bookModelDate;
            }
        }

    }
}
