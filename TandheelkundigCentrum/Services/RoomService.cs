using Microsoft.AspNetCore.Mvc;
using TandheelkundigCentrum.Data.Base;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Data;

namespace TandheelkundigCentrum.Services;

public class RoomService(ApplicationDbContext context) : BaseRepository<Room, int>(context)
{

}
