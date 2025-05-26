using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Models.ViewModels
{
    public class AppointmentViewModel
    {
        [Required]
        public string? ProviderId { get; set; }

        [Required]
        public string? Department { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public string? Description { get; set; }

        public SelectList? Providers { get; set; }
        public List<string>? SelectedDoctors { get; set; } = new();
        public List<TimeSpan>? AvailableTimeSlots { get; set; }
        public List<DoctorDisplay>? ProviderList { get; set; }
    }
}
