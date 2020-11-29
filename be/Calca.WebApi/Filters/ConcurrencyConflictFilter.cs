using Calca.Infrastructure.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Calca.WebApi.Filters
{
    public class ConcurrencyConflictFilter : ActionFilterAttribute
    {
        public string EntityType { get; }

        public ConcurrencyConflictFilter(string entityType)
        {
            EntityType = entityType;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if (context.Exception is ConcurrencyConflictException ex)
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ConcurrencyConflictFilter>>();
                logger.LogWarning(ex, "Concurrency conflict when updating entity {EntityType}", EntityType);
                context.Result = new ObjectResult(new
                {
                    error = "concurrency_conflict",
                    errorDescription = Message
                });
                context.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                context.ExceptionHandled = true;
            }
        }

        protected virtual string Message
        {
            get
            {
                return 
                    $"An attempt was made to modify an entity of type {EntityType} based on outdated information. " +
                    $"Please reload the entity and try again";
            }
        }
    }
}
