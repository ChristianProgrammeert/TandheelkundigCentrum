using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TandheelkundigCentrum.Data;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Models;
using TandheelkundigCentrum.Services;

namespace TandheelkundigCentrum.Controllers
{
    public class AppointmentController(AppointmentService appointmentService) : Controller
    {

        // Action to display a list of appointments
        public async Task<IActionResult> Index()
        {
            // Retrieve appointments from the service
            List<Appointment> appointments = await appointmentService.GetAllAppointments();
            return View(appointments);
        }
        
        public async Task<IActionResult> Patient()
        {
            var token = HttpContext.Request.Cookies["Token"];
            var id = new JwtService().GetUserId(token);
            // Retrieve appointments from the service
            List<Appointment> user_appointments = await appointmentService.GetUserAppointments(Guid.Parse(id));
            return View(user_appointments);
        }

        public async Task<IActionResult> Dentist()
        {
            var token = HttpContext.Request.Cookies["Token"];
            var id = new JwtService().GetUserId(token);
            // Retrieve appointments from the service
            List<Appointment> dentist_appointments = await appointmentService.GetDentistAppointments(Guid.Parse(id));
            return View(dentist_appointments);
        }

        public async Task<IActionResult> CreateAppointmentAsync()
        {
            // Populate dropdown lists or other necessary data
            var model = new CreateAppointmentViewModel
            {
                Treatments = await appointmentService.GetAllTreatments(),
                Dentists = await appointmentService.GetAllDentists(),
                Patients = await appointmentService.GetAllPatients()
            };

            return View("EditAppointment", model); // Use the same view for create and edit
        }

        // Action to handle the submission of the appointment creation form
        [HttpPost]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentViewModel model)
        {
            return await EditAppointment(model); // Treat create and edit similarly
        }

        // Action to display the edit form for an existing appointment
        public async Task<IActionResult> EditAppointment(int? appointmentId)
        {
            if (appointmentId == null)
            {
                // Handle null appointment ID (e.g., return error view)
            }

            // Retrieve existing appointment from the database
            Appointment appointment = await appointmentService.GetByIdAsync(appointmentId.Value);
            if (appointment == null)
            {
                // Handle non-existent appointment (e.g., return error view)
            }

            // Populate dropdown lists or other necessary data
            var model = new CreateAppointmentViewModel
            {
                Treatments = await appointmentService.GetAllTreatments(),
                Dentists = await appointmentService.GetAllDentists(),
                Patients = await appointmentService.GetAllPatients(),
                // Set properties from existing appointment for editing
                SelectedTreatmentIds = appointment.Treatments.Select(t => t.Id).ToList(),
                DentistId = appointment.DentistId,
                PatientId = appointment.PatientId,
                RoomId = appointment.RoomId,
                DateTime = appointment.DateTime,
                Note = appointment.Note
            };

            return View("EditAppointment", model); // Use the same view for create and edit
        }

        // Action to handle the submission of the appointment edit form
        [HttpPost]
        public async Task<IActionResult> EditAppointment(CreateAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Update appointment using the service
                // Here you would call a service method to update the appointment
                // You can access the appointment properties from the model
                // For example: appointmentService.Update(model)

                // After updating, redirect to appointments list or details page
                return RedirectToAction("Appointments");
            }

            // If model state is not valid, return to the form with validation errors
            model.Treatments = await appointmentService.GetAllTreatments();
            model.Dentists = await appointmentService.GetAllDentists();
            model.Patients = await appointmentService.GetAllPatients();

            return View("EditAppointment", model); // Use the same view for create and edit
        }

        public async Task<ViewResult> View(int id)
        {
            return View(await appointmentService.GetAppointmentById(id));
        }

    }
}
