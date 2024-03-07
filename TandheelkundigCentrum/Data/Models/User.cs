using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using TandheelkundigCentrum.Data.Base;

namespace TandheelkundigCentrum.Data.Models;

public class User : IBaseEntity<Guid>
{
    [Key] public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Prefix { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public DateOnly Birthdate { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? Insurer { get; set; }
    public Collection<Appointment> Appointments { get; set; }
    public Collection<Group> Groups { get; internal set; }
}