# Doctor System

A comprehensive healthcare management system built with ASP.NET Core MVC that facilitates doctor-patient interactions, appointment scheduling, and medical record management.

## Features

### For Patients
- **Appointment Management**
  - Book appointments with up to 3 doctors simultaneously
  - View and manage upcoming appointments
  - Cancel appointments with reason tracking
  - Receive email notifications for appointment confirmations

- **Medical Records**
  - Access complete medical history
  - View test results and diagnoses
  - Track prescriptions and medications
  - Download medical documents

- **Doctor Selection**
  - Browse doctors by department
  - View doctor profiles and specialties
  - Check doctor availability in real-time

### For Doctors
- **Availability Management**
  - Set recurring and one-time availability slots
  - Manage appointment schedule
  - View patient appointments
  - Track appointment history

- **Patient Care**
  - Create and manage prescriptions
  - Record test results
  - Access patient medical history
  - Send secure messages to patients

### Security Features
- Role-based access control (Patient, Doctor, Admin)
- Secure authentication and authorization
- Audit logging for sensitive operations
- Data encryption for medical records

## Technical Stack

- **Backend**: ASP.NET Core MVC
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Frontend**: 
  - Bootstrap 5
  - jQuery
  - Font Awesome icons
- **Email Service**: SMTP integration
- **Validation**: FluentValidation

## Prerequisites

- .NET 6.0 SDK or later
- SQL Server 2019 or later
- Visual Studio 2022 or later (recommended)
- SMTP server for email notifications

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/DoctorSystem.git
   ```

2. Update the connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Your_Connection_String_Here"
   }
   ```

3. Update SMTP settings in `appsettings.json`:
   ```json
   "SmtpSettings": {
     "Server": "your.smtp.server",
     "Port": 587,
     "Username": "your_username",
     "Password": "your_password"
   }
   ```

4. Run database migrations:
   ```bash
   dotnet ef database update
   ```

5. Run the application:
   ```bash
   dotnet run
   ```

## Project Structure

```
DoctorSystem/
├── Controllers/         # MVC Controllers
├── Models/             # Domain Models
├── ViewModels/         # View-specific Models
├── DTOs/              # Data Transfer Objects
├── Services/          # Business Logic
│   ├── Interfaces/    # Service Contracts
│   └── Implementations/ # Service Implementations
├── Data/              # Database Context
├── Views/             # Razor Views
├── wwwroot/          # Static Files
└── Validators/       # FluentValidation Rules
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support, please open an issue in the GitHub repository or contact the development team.
