# Appointment Booking Feature Documentation

## Overview
The Appointment Booking feature allows patients to schedule appointments with multiple doctors. The system supports selecting up to 3 doctors, choosing a department, and scheduling within business hours (09:00-17:00).

## Architecture

### Components
1. **View Model (AppointmentViewModel)**
   - Handles form data binding
   - Manages doctor selection (max 3)
   - Validates time slots and dates
   - Contains available time slots and doctor list

2. **DTO (CreateAppointmentDto)**
   - Transfers data between layers
   - Supports multiple doctor selection
   - Includes department and time information

3. **Controller (AppointmentsController)**
   - Handles GET/POST requests
   - Manages form submission
   - Coordinates between view and service

4. **Service (AppointmentService)**
   - Business logic implementation
   - Database operations
   - Appointment creation and management

### Validation
- **Server-side**: Using Data Annotations and FluentValidation
- **Client-side**: JavaScript validation for immediate feedback
- **Time Validation**: Ensures appointments within 09:00-17:00
- **Doctor Selection**: Limits to 1-3 doctors

### Database Schema
```sql
Appointments
- Id (PK)
- DoctorId (FK)
- PatientId (FK)
- Department
- StartTime
- EndTime
- Notes
- Status
```

## Usage

### Booking an Appointment
1. Navigate to the appointment booking page
2. Select department
3. Choose appointment date
4. Select start and end times
5. Choose up to 3 doctors
6. Add optional notes
7. Submit the form

### Validation Rules
- Department: Required
- Date: Must be today or future
- Time: Between 09:00-17:00
- End Time: Must be after start time
- Doctors: 1-3 selections required
- Notes: Optional, max 500 characters

### Error Handling
- Form validation errors display inline
- Server-side validation errors show at top
- Client-side validation provides immediate feedback

## Security
- Requires authentication
- Implements anti-forgery tokens
- Validates user permissions
- Sanitizes input data

## Dependencies
- ASP.NET Core
- Entity Framework Core
- FluentValidation
- jQuery (for client-side validation)

## Future Improvements
1. Add doctor availability checking
2. Implement appointment conflict detection
3. Add email notifications
4. Support recurring appointments
5. Add appointment modification feature 