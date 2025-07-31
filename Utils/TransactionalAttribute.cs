using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace SSO_Api.Utils
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SSO_Api.Data;

    public class TransactionalAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetService<AppDbContext>();

            using var transaction = await dbContext.Database.BeginTransactionAsync();

            var resultContext = await next(); // gọi action

            if (resultContext.Exception == null || resultContext.ExceptionHandled)
            {
                await transaction.CommitAsync();
            }
            else
            {
                await transaction.RollbackAsync();
            }
        }
    }

}
