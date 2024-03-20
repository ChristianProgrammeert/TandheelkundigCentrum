using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using TandheelkundigCentrum.Data.Base;

namespace TandheelkundigCentrum.Data.Models;

public class Treatment : IBaseEntity<int>
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public TimeSpan Duration { get; set; }
    public double Price { get; set; }
    public bool Archived { get; set; }
    public Collection<Rating> Ratings { get; internal set; }

    public Collection<Appointment> Appointments { get; set; }
}
