using Microsoft.AspNetCore.Mvc.Filters;
using TandheelkundigCentrum.Data;
using TandheelkundigCentrum.Services;

namespace TandheelkundigCentrum.Filters;

public class AuthFilter(ApplicationDbContext context) : IAuthorizationFilter
{
    private readonly AuthService Service = new(context);
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Request.Cookies.ContainsKey("Token") ||
            !Service.ValidateToken(context.HttpContext.Request.Cookies["Token"]))
        {
            context.HttpContext.Response.Redirect("/Auth/Login/");
        }
    }
}