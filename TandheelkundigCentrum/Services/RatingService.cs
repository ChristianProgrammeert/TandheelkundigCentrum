using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using TandheelkundigCentrum.Data.Base;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Data;
using Microsoft.EntityFrameworkCore;

namespace TandheelkundigCentrum.Services;

public class RatingService(ApplicationDbContext context) : BaseRepository<Rating, int>(context)
{

    public async Task<Rating?> GetRating(int id)
    {
        return await GetByIdAsync(id);
    }

    /// <summary>
    /// Get the treatment with the given id
    /// </summary>
    public async Task<List<Treatment>> GetUserTreatments(Guid user_id)
    {
        // Fetch all appointment IDs where the given user is either the dentist or the patient
        var userAppointmentIds = await context.Appointments
                                              .Where(a => a.DentistId == user_id || a.PatientId == user_id)
                                              .Select(a => a.Id)
                                              .ToListAsync();
        // Now fetch all treatments connected to these appointments
        var treatments = await context.Treatments
                                      .Where(t => t.Appointments.Any(a => userAppointmentIds.Contains(a.Id)))
                                      .ToListAsync();
        return treatments;
    }
}
