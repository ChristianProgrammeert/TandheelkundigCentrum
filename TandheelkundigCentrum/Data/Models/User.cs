using System.ComponentModel.DataAnnotations;
using TandheelkundigCentrum.Data.Base;

namespace TandheelkundigCentrum.Data.Model;

public class User : IBaseEntity<Guid>
{
    [Key] public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public DateOnly Birtdate { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? Insurer { get; set; }
}