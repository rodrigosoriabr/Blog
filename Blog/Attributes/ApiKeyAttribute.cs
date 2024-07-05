using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Query.TryGetValue(
                Configuration.ApiKeyName!, out var extractedApiKey))
        {
            context.Result = new ContentResult
            {
                StatusCode = 401,
                Content = "ApiKey not found"
            };

            return;
        }

        if (!Configuration.ApiKey!.Equals(extractedApiKey))
        {
            context.Result = new ContentResult
            {
                StatusCode = 403,
                Content = "Not authorised"
            };

            return;
        }

        await next();
    }
}