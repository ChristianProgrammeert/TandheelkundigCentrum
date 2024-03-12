using Microsoft.AspNetCore.Mvc;
using TandheelkundigCentrum.Data.Base;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Data;

namespace TandheelkundigCentrum.Services;

public class RoomService(ApplicationDbContext context) : BaseRepository<Room, int>(context)
{
    /// <summary>
    /// Get the room with the given id and its patients
    /// </summary>
    public async Task<Room?> GetRoom(int id)
    {
        return await GetByIdAsync(id);
    }
}
