using Microsoft.EntityFrameworkCore;
using TandheelkundigCentrum.Data;
using TandheelkundigCentrum.Data.Base;
using TandheelkundigCentrum.Data.Models;

namespace TandheelkundigCentrum.Services;

public class UserService(ApplicationDbContext context) : BaseRepository<User, Guid>(context)
{
    public async Task<List<User>> GetAllColleagues()
    {
        var colleagues = await context.Users.Where(u =>
                u.Groups.Any(
                    g => g.Name == Group.GroupName.Assistent || g.Name == Group.GroupName.Dentist
                    )
                )
            .Include(u => u.Groups)
            .ToListAsync();
        return colleagues;
    }
}