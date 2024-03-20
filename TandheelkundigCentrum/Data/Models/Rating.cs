using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TandheelkundigCentrum.Data.Base;

namespace TandheelkundigCentrum.Data.Models;

public class Rating : IBaseEntity<int>
{
    [Key] public int Id { get; set; }
    public int TreatmentId {  get; set; }
    public Treatment Treatment { get; set; }
    public int Amount { get; set; }

    internal static void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Rating>()
            .HasOne(r => r.Treatment)
            .WithMany(t => t.Ratings)
            .HasForeignKey(r => r.TreatmentId);
    }
}