using Microsoft.AspNetCore.Mvc;
using TandheelkundigCentrum.Data;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Filters;
using TandheelkundigCentrum.Services;

namespace TandheelkundigCentrum.Controllers
{
    [AuthFilter(Group.GroupName.Admin, Group.GroupName.Assistent, Group.GroupName.Dentist)]
    public class TreatmentController : Controller
    {
        private readonly TreatmentService treatmentService;
        public TreatmentController(ApplicationDbContext context) { 
            treatmentService = new TreatmentService(context);
        }
        public async Task<IActionResult> IndexAsync()
        {
            var rooms = await treatmentService.GetAllAsync();
            return View(rooms);
        }

        /// <summary>
        /// View edit page with the treatment with the given id, if id is given.
        /// </summary>
        public async Task<IActionResult> Edit(int? treatmentId)
        {
            Treatment? treatment = treatmentId == null
                ? null
                : await treatmentService.GetByIdAsync(treatmentId.Value);

            return View(treatment);
        }

        /// <summary>
        /// When an edit is done, add it to the service when id is not set, else update the treatment with the given id.
        /// Redirect to the view page of the new or updated treatment.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Treatment treatment)
        {
            if (id == null)
            {
                treatment.Archived = false;
                treatment = await treatmentService.AddAsync(treatment);
            }
            else
            {
                treatment.Archived = true;
                treatment = await treatmentService.UpdateAsync(treatment);
            }
            return RedirectToAction("View", new { id = treatment.Id });
        }

        /// <summary>
        /// Delete the treatment with the given id and redirect to the index view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            await treatmentService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// View view page with the treatment with the given id.
        /// </summary>
        public async Task<ViewResult> View(int id)
        {
            return View(await treatmentService.GetTreatment(id));
        }
    }
}
