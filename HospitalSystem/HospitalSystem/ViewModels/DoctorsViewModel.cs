using HospitalSystem.Models;

namespace HospitalSystem.ViewModels;

public class DoctorsViewModel
{
    public List<Doctor> Doctors { get; set; } = new();
    public List<string> Specializations { get; set; } = new();

    public string? Name { get; set; }
    public string? Specialization { get; set; }

    public int Page { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
}
