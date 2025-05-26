using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DoctorSystem.Models;

namespace DoctorSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentAuditLog> AppointmentAuditLogs { get; set; }
        public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Appointment>()
                .HasOne(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Provider)
                .WithMany()
                .HasForeignKey(a => a.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AppointmentAuditLog>()
                .HasOne(a => a.Appointment)
                .WithMany()
                .HasForeignKey(a => a.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AppointmentAuditLog>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
