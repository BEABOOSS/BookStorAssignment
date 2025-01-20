using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookStore.Filters
{

    public class FeatureEnabledAttribute : Attribute
    {
        public bool IsEnabled { get; set; }
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!IsEnabled)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
