using System.Globalization;
using HospitalSystem.Data;
using HospitalSystem.Models;
using HospitalSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Controllers;

public class AppointmentsController : Controller
{
    private readonly AppDbContext _db;

    public AppointmentsController(AppDbContext db) => _db = db;

    // Every 30 minutes from 9:00 AM to 4:30 PM
    private static List<string> BuildTimeSlots()
    {
        var slots = new List<string>();
        for (var t = new TimeSpan(9, 0, 0); t <= new TimeSpan(16, 30, 0); t = t.Add(TimeSpan.FromMinutes(30)))
            slots.Add(t.ToString(@"hh\:mm"));
        return slots;
    }

    // GET: /Appointments  -> all booked appointments
    public async Task<IActionResult> Index()
    {
        var list = await _db.Appointments
            .Include(a => a.Doctor)
            .OrderBy(a => a.Date).ThenBy(a => a.Time)
            .ToListAsync();
        return View(list);
    }

    // GET: /Appointments/Create?doctorId=1
    public async Task<IActionResult> Create(int doctorId)
    {
        var doctor = await _db.Doctors.FindAsync(doctorId);
        if (doctor == null) return NotFound();

        return View(FillDoctor(new BookAppointmentViewModel(), doctor));
    }

    // POST: /Appointments/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookAppointmentViewModel vm)
    {
        var doctor = await _db.Doctors.FindAsync(vm.DoctorId);
        if (doctor == null) return NotFound();
        FillDoctor(vm, doctor);

        TimeSpan time = default;

        if (ModelState.IsValid)
        {
            var date = vm.Date!.Value.Date;

            if (date < DateTime.Today)
                ModelState.AddModelError(nameof(vm.Date), "You cannot book a date in the past.");
            else if (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
                ModelState.AddModelError(nameof(vm.Date), "Appointments are available Sunday to Thursday only.");

            if (!vm.TimeSlots.Contains(vm.Time!) ||
                !TimeSpan.TryParseExact(vm.Time, @"hh\:mm", CultureInfo.InvariantCulture, out time))
                ModelState.AddModelError(nameof(vm.Time), "Please choose a valid time slot.");

            if (ModelState.IsValid)
            {
                bool taken = await _db.Appointments.AnyAsync(a =>
                    a.DoctorId == vm.DoctorId && a.Date == date && a.Time == time);

                if (taken)
                    ModelState.AddModelError(nameof(vm.Time),
                        "This doctor is already booked at this date and time. Please choose another slot.");
            }
        }

        if (!ModelState.IsValid) return View(vm);

        _db.Appointments.Add(new Appointment
        {
            PatientName = vm.PatientName.Trim(),
            DoctorId = vm.DoctorId,
            Date = vm.Date!.Value.Date,
            Time = time
        });
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private static BookAppointmentViewModel FillDoctor(BookAppointmentViewModel vm, Doctor doctor)
    {
        vm.DoctorId = doctor.Id;
        vm.DoctorName = doctor.Name;
        vm.Specialization = doctor.Specialization;
        vm.ImagePath = doctor.ImagePath;
        vm.TimeSlots = BuildTimeSlots();
        return vm;
    }
}
