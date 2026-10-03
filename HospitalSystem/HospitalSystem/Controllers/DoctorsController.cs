using HospitalSystem.Data;
using HospitalSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Controllers;

public class DoctorsController : Controller
{
    private const int PageSize = 3;
    private readonly AppDbContext _db;

    public DoctorsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? name, string? specialization, int page = 1)
    {
        var query = _db.Doctors.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(d => d.Name.Contains(name.Trim()));

        if (!string.IsNullOrWhiteSpace(specialization))
            query = query.Where(d => d.Specialization == specialization);

        int total = await query.CountAsync();
        int totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        var vm = new DoctorsViewModel
        {
            Doctors = await query.OrderBy(d => d.Name)
                                 .Skip((page - 1) * PageSize)
                                 .Take(PageSize)
                                 .ToListAsync(),
            Specializations = await _db.Doctors.Select(d => d.Specialization)
                                               .Distinct().OrderBy(s => s).ToListAsync(),
            Name = name,
            Specialization = specialization,
            Page = page,
            TotalPages = totalPages
        };

        return View(vm);
    }
}
