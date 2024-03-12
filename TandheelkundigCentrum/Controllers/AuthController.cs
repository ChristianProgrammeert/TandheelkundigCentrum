using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TandheelkundigCentrum.Data;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Models;
using TandheelkundigCentrum.Services;

namespace TandheelkundigCentrum.Controllers;

public class AuthController(ApplicationDbContext context) : Controller
{
    private AuthService service = new(context);
    private JwtService jwtService = new();

    public IActionResult Login()
    {
        if (Request.Cookies.ContainsKey("Token") && jwtService.ValidateToken(Request.Cookies["Token"]))
            return RedirectToAction("Index", "Home");
        return View(new LoginViewModel());
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid) return View(loginViewModel);
        var user = await service.GetUserByEmailAndPassword(loginViewModel.Email, loginViewModel.Password);
        if (user == null)
        {
            ModelState.AddModelError("Error", "Het gegeven email of wachtwoord is onjuist.");
            return View(loginViewModel);
        }

        SetTokenCookie(user);
        return RedirectToAction("Index", "Home");
    }
    
    public IActionResult Logout()
    {
        SetTokenCookie(null);
        return RedirectToAction("Login");
    }

    public async Task<IActionResult> WhoAmI()
    {
        if (!Request.Cookies.ContainsKey("Token") || !jwtService.ValidateToken(Request.Cookies["Token"])) 
            return View();
        var id = jwtService.GetUserId(Request.Cookies["Token"]);
        return View(await service.GetByIdAsync(Guid.Parse(id)));
    }
    
    private void SetTokenCookie(User? user)
    {
        var cookieOptions = new CookieOptions { HttpOnly = true };
        if (user == null)
            Response.Cookies.Delete("Token");
        else
            Response.Cookies.Append("Token", jwtService.GenerateToken(user), cookieOptions);
    }
}