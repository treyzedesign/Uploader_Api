using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Uploader_Api.Helpers
{
    public class CustomTokenHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (context.Resource is AuthorizationFilterContext authorizationFilterContext)
            {
                authorizationFilterContext.Result = new StatusCodeResult(401);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
