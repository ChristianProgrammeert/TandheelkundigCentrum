using Microsoft.EntityFrameworkCore;
using TandheelkundigCentrum.Data.Models;

namespace TandheelkundigCentrum.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Room> Rooms { get; set; }

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);
        Appointment.OnModelCreating(model);
        Group.OnModelCreating(model);
        Rating.OnModelCreating(model);
        User.OnModelCreating(model);
    }
}