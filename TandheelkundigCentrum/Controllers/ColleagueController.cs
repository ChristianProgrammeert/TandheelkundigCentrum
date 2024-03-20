using Microsoft.AspNetCore.Mvc;
using TandheelkundigCentrum.Data;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Filters;
using TandheelkundigCentrum.Services;
using TandheelkundigCentrum.Utilities;
using static TandheelkundigCentrum.Data.Models.Group;

namespace TandheelkundigCentrum.Controllers;

public class ColleagueController(ApplicationDbContext context) : Controller
{
    private UserService userService = new(context);
    private JwtService jwtService = new();

    public async Task<IActionResult> Index()
    {
        var colleagues = await userService.GetAllColleagues();
        return View(colleagues);
    }

    [AuthFilter(GroupName.Admin)]
    public async Task<IActionResult> Edit(Guid? colleagueId)
    {
        var user = colleagueId.HasValue
            ? await userService.GetByIdAsync(colleagueId.Value, u => u.Groups)
            : new User();

        return View(user);
    }


    [HttpPost]
    [AuthFilter(GroupName.Admin)]
    public async Task<IActionResult> Edit(User model)
    {
        if (!ModelState.IsValid)
            return View(model);
        return RedirectToAction("View", new { guid = model.Id });
    }

    [AuthFilter(GroupName.Admin)]
    public async Task<IActionResult> Create()
    {
        var user = new User
        {
            Groups = []
        };
        return View(user);
    }


    [HttpPost]
    [AuthFilter(GroupName.Admin)]
    public async Task<IActionResult> Create(User model)
    {
        model.Groups = [];
        foreach (var groupName in Enum.GetValues<GroupName>())
            if (HttpContext.Request.Form.ContainsKey(groupName + "-input") )
                model.Groups.Add(new Group { Name = groupName });
        if (!ModelState.IsValid) return View(model);
        model.Password = PasswordHasher.HashPassword(model.Password);
        await userService.AddAsync(model);
        return RedirectToAction("View");
    }

    [AuthFilter(GroupName.Admin)]
    public IActionResult View(Guid? guid)
    {
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(Guid id)
    {
        await userService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}