using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TandheelkundigCentrum.Data.Models;

namespace TandheelkundigCentrum.Models
{
    public class CreateAppointmentViewModel
    {
        [Required(ErrorMessage = "Selecteer minstens één behandeling")]
        public List<int> SelectedTreatmentIds { get; set; } // Change to List<int> to store multiple treatment IDs

        [Required(ErrorMessage = "Selecteer een tandarts")]
        public Guid DentistId { get; set; }

        [Required(ErrorMessage = "Selecteer een patiënt")]
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "Selecteer een kamer")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Selecteer een datum")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Voeg een notitie toe")]
        public string Note { get; set; }

        // Dropdown lists for selection
        public List<Treatment> Treatments { get; set; }
        public List<User> Dentists { get; set; }
        public List<User> Patients { get; set; }
        public List<Room> Rooms { get; set; }
    }
}