using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using TandheelkundigCentrum.Data.Base;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Data;

namespace TandheelkundigCentrum.Services;

public class TreatmentService(ApplicationDbContext context) : BaseRepository<Treatment, int>(context)
{
    /// <summary>
    /// Get the treatment with the given id
    /// </summary>
    public async Task<Treatment?> GetTreatment(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<Collection<Treatment>> FindByIds(IEnumerable<int> treatmentIds)
    {
        Collection<Treatment> treatments = [];
        foreach (var id in treatmentIds)
            treatments.Add(await GetByIdAsync(id));
        return treatments;
    }
}
