using Microsoft.AspNetCore.Mvc;
using TandheelkundigCentrum.Data;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Services;

namespace TandheelkundigCentrum.Controllers
{
    public class RoomController : Controller
    {
        private readonly RoomService roomService;
        public RoomController(ApplicationDbContext context) { 
            roomService = new RoomService(context);
        }
        public async Task<IActionResult> IndexAsync()
        {
            var rooms = await roomService.GetAllAsync();
            return View(rooms);
        }
    }
}
