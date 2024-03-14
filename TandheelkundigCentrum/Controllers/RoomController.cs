using Microsoft.AspNetCore.Mvc;
using TandheelkundigCentrum.Data;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Filters;
using TandheelkundigCentrum.Services;

namespace TandheelkundigCentrum.Controllers
{
    [AuthFilter(Group.GroupName.Admin, Group.GroupName.Assistent, Group.GroupName.Dentist)]
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

        /// <summary>
        /// View edit page with the room with the given id, if id is given.
        /// </summary>
        public async Task<IActionResult> Edit(int? roomId)
        {
            Room? room = roomId == null
                ? null
                : await roomService.GetByIdAsync(roomId.Value);
            return View(room);
        }

        /// <summary>
        /// When an edit is done, add it to the service when id is not set, else update the room with the given id.
        /// Redirect to the view page of the new or updated room.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Room room)
        {
            room = id == null
                ? await roomService.AddAsync(room)
                : await roomService.UpdateAsync(room);
            return RedirectToAction("View", new { id = room.Id });
        }

        /// <summary>
        /// Delete the room with the given id and redirect to the index view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            await roomService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// View view page with the room with the given id.
        /// </summary>
        public async Task<ViewResult> View(int id)
        {
            return View(await roomService.GetRoom(id));
        }
    }
}
