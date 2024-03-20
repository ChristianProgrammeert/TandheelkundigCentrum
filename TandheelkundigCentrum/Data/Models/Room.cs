using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using TandheelkundigCentrum.Data.Base;

namespace TandheelkundigCentrum.Data.Models;

public class Room : IBaseEntity<int>
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public Collection<Appointment> Appointments { get; set; }   
}