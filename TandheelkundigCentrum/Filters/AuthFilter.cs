using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TandheelkundigCentrum.Data;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Services;

namespace TandheelkundigCentrum.Filters;

public class AuthFilter(params Group.GroupName[] groups) : ActionFilterAttribute
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
                context.HttpContext.Response.Redirect("/Auth/Login/");
            return;
        }

        var token = context.HttpContext.Request.Cookies["Token"];
        context.HttpContext.User = Service.GetClaimsIdentity(token);
        if (Service.GetUserGroups(token)?.Any(groups.Contains) == true) return;
        context.HttpContext.Response.Clear();
        context.Result = new UnauthorizedObjectResult("Unauthorized");
    }
}