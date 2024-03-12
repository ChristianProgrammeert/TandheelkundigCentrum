using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using TandheelkundigCentrum.Data.Base;

namespace TandheelkundigCentrum.Data.Models;

public class Appointment : IBaseEntity<int>
{
    [Key] public int Id { get; set; }
    public Guid DentistId { get; set; }
    public User Dentist { get; set; }
    public Guid PatientId { get; set; }
    public User Patient { get; set; }
    public int RoomtId { get; set; }
    public Room Room { get; set; }
    public DateTime DateTime { get; set; }
    public string Note { get; set; }

    internal static void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Appointment>()
            .HasOne(a => a.Dentist)
            .WithMany(u => u.DentistAppointments)
            .HasForeignKey(a => a.DentistId)
            .OnDelete(DeleteBehavior.NoAction);


        model.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(u => u.PatientAppointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.NoAction);


        model.Entity<Appointment>()
            .HasOne(a => a.Room)
            .WithMany(r => r.Appointments)
            .HasForeignKey(a => a.RoomtId);
    }
}
