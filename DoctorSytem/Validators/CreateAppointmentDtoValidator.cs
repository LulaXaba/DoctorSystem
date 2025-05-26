using FluentValidation;
using DoctorSystem.DTOs.Appointments;

namespace DoctorSystem.Validators
{
    /// <summary>
    /// Validator for CreateAppointmentDto.
    /// Implements business rules and validation logic for appointment creation.
    /// </summary>
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentDtoValidator()
        {
            // Validate doctor selection
            RuleFor(x => x.DoctorIds)
                .NotEmpty().WithMessage("Please select at least one doctor.")
                .Must(x => x.Count <= 3).WithMessage("You can select a maximum of 3 doctors.");

            // Validate department
            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required.");

            // Validate appointment date
            RuleFor(x => x.AppointmentDate)
                .NotEmpty().WithMessage("Appointment date is required.")
                .Must(date => date.Date >= DateTime.Today)
                .WithMessage("Appointment date must be today or in the future.");

            // Validate start time
            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.")
                .Must(time => time.Hours >= 9 && time.Hours < 17)
                .WithMessage("Start time must be between 09:00 and 17:00.");

            // Validate end time
            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .Must(time => time.Hours >= 9 && time.Hours < 17)
                .WithMessage("End time must be between 09:00 and 17:00.")
                .Must((dto, time) => time > dto.StartTime)
                .WithMessage("End time must be after start time.");

            // Validate notes length
            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Notes cannot exceed 500 characters.");
        }
    }
} 