using BlazorPracticeApp.Api.ContextDatabase;
using BlazorPracticeApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BlazorPracticeApp.Api.JWT
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RoleAutorizeAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int[] roleId;
        public RoleAutorizeAttribute(int[] _roleId)
        {
            roleId = _roleId;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var db = context.HttpContext.RequestServices.GetRequiredService<ContextDb>();
            string? token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new JsonResult(new { error = "Сессия не передана" }) { StatusCode = 401 };
                return;
            }

            var session = db.Sessions.Include(p => p.Users).FirstOrDefault(p => p.Token == token);

            if (session == null)
            {
                context.Result = new JsonResult(new { error = "Сессия не передана" }) { StatusCode = 401 };
                return;
            }

            context.HttpContext.Items["CurrentUserId"] = session.UserId;

            if (session.Users == null ||
               session.Users.RoleId == null ||
               !roleId.Contains(session.Users.RoleId.Value))
            {
                context.Result = new JsonResult(new { error = "Недостаточно прав" }) { StatusCode = 403 };
                return;
            }

            await next();
        }
    }
}