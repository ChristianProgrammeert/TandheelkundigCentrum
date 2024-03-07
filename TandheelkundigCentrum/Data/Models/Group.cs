using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TandheelkundigCentrum.Data.Base;

namespace TandheelkundigCentrum.Data.Models;

public class Group : IBaseEntity<int>
{
    [Key] public int Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public GroupName Name { get; set; }
    public enum GroupName
    {
        Admin,
        Dentist,
        Assistent,
        Patient,
    }

    internal static void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Group>()
            .HasOne(g => g.User)
            .WithMany(u => u.Groups)
            .HasForeignKey(g => g.UserId);
    }
}