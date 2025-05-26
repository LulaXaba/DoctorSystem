using DoctorSystem.Models;

namespace DoctorSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetDoctorsAsync();
    }
} 