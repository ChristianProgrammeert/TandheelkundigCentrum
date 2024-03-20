using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TandheelkundigCentrum.Data.Base;
using TandheelkundigCentrum.Utilities;

namespace TandheelkundigCentrum.Data.Models;

[Index(nameof(Email), IsUnique = true)]
public class User : IBaseEntity<Guid>
{
    [Key] public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Prefix { get; set; }

    [NotMapped]
    public string Fullname => $"{Prefix} {LastName}, {FirstName}".Trim(' ').Trim(',');

    public string Password { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public DateOnly Birthdate { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? Insurer { get; set; }
    public Collection<Appointment> DentistAppointments { get; set; }
    public Collection<Appointment> PatientAppointments { get; set; }
    public Collection<Group> Groups { get; internal set; }

    [NotMapped]
    public List<Appointment> Appointments
    {
        get => [.. DentistAppointments, .. PatientAppointments];
    }


    internal static void OnModelCreating(ModelBuilder model)
    {
        var guid = Guid.NewGuid();
        model.Entity<User>().HasData(
            new User
            {
                Id = guid,
                Prefix = "Dhr.",
                FirstName = "Admin",
                LastName = "Istrator",
                Email = "admin@tandheelkundigcentrum.nl",
                Password = PasswordHasher.HashPassword("admin"),
                Birthdate = DateOnly.MinValue,
                Address = "",
                City = "",
                PostalCode = "",
                Insurer = null,
                Phone = "",
            });
        model.Entity<Group>().HasData(
            new Group
            {
                Id = 1,
                UserId = guid,
                Name = Group.GroupName.Admin,
            });
    }
}