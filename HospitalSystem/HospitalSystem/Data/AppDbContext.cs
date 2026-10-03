using HospitalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Prevent double booking at the database level too
        modelBuilder.Entity<Appointment>()
            .HasIndex(a => new { a.DoctorId, a.Date, a.Time })
            .IsUnique();

        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { Id = 1, Name = "Dr. Ahmed Hassan",  Specialization = "Cardiology",      ImagePath = "/images/doctor1.jpg" },
            new Doctor { Id = 2, Name = "Dr. Omar Khalil",   Specialization = "Neurology",       ImagePath = "/images/doctor2.jpg" },
            new Doctor { Id = 3, Name = "Dr. Mahmoud Saeed", Specialization = "Orthopedics",     ImagePath = "/images/doctor3.jpg" },
            new Doctor { Id = 4, Name = "Dr. Youssef Adel",  Specialization = "Dermatology",     ImagePath = "/images/doctor4.jpg" },
            new Doctor { Id = 5, Name = "Dr. Karim Mostafa", Specialization = "Pediatrics",      ImagePath = "/images/doctor5.jpg" },
            new Doctor { Id = 6, Name = "Dr. Tarek Ibrahim", Specialization = "General Surgery", ImagePath = "/images/doctor6.jpg" }
        );
    }
}
