using DoctorSystem.Data;
using DoctorSystem.Models;
using DoctorSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoctorSystem.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationUser>> GetDoctorsAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "Doctor")
                .ToListAsync();
        }
    }
} 