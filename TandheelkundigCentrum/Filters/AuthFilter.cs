using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TandheelkundigCentrum.Data;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Services;

namespace TandheelkundigCentrum.Filters;

public class AuhFilter(params Group.GroupName[] groups) : ActionFilterAttribute
{
    private readonly JwtService Service = new();

    private string[] allowedPaths =
    [
        "/auth/login",
    ];

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        if (!context.HttpContext.Request.Cookies.ContainsKey("Token") ||
            !Service.ValidateToken(context.HttpContext.Request.Cookies["Token"]))
        {
            if (allowedPaths.All(s => s != context.HttpContext.Request.Path))
                context.HttpContext.Response.Redirect($"Auth/Login/");
            return;
        }

        context.HttpContext.User = Service.GetClaimsIdentity(context.HttpContext.Request.Cookies["Token"]);
        if (context.HttpContext.User.Claims.Any(
                c => c.Type == "role" && groups.Any(g => g.ToString() == c.Value)
            )) return;
        context.HttpContext.Response.Clear();
        context.Result = new UnauthorizedObjectResult("Unauthorized");
    }
}