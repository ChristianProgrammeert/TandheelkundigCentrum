using Microsoft.AspNetCore.Mvc;
using TandheelkundigCentrum.Data;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Filters;
using TandheelkundigCentrum.Services;

namespace TandheelkundigCentrum.Controllers
{
    [AuthFilter(Group.GroupName.Patient, Group.GroupName.Admin)]
    public class RatingController : Controller
    {
        private readonly RatingService ratingService;
        public RatingController(ApplicationDbContext context) { 
            ratingService = new RatingService(context);
        }
        public async Task<IActionResult> IndexAsync()
        {
            var ratings = await ratingService.GetAllAsync(r => r.Treatment);
            return View(ratings);
        }

        private async Task fillViewBag()
        {
            var token = HttpContext.Request.Cookies["Token"];
            var id = new JwtService().GetUserId(token);
            ViewBag.UserTreatments = await ratingService.GetUserTreatments(Guid.Parse(id));
        }

        /// <summary>
        /// View edit page with the treatment with the given id, if id is given.
        /// </summary>
        public async Task<IActionResult> Rate(int? ratingId)
        {
            await fillViewBag();
            Rating? rating = ratingId == null
                ? null
                : await ratingService.GetByIdAsync(ratingId.Value);

            return View(rating);
        }

        /// <summary>
        /// When an edit is done, add it to the service when id is not set, else update the treatment with the given id.
        /// Redirect to the view page of the new or updated treatment.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Rate(int? id, Rating rating)
        {
            if (id == null)
            {
                rating = await ratingService.AddAsync(rating);
            }
            else
            {
                rating = await ratingService.UpdateAsync(rating);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Delete the treatment with the given id and redirect to the index view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            await ratingService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
