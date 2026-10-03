using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.ViewModels;

public class BookAppointmentViewModel
{
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;

    [Required(ErrorMessage = "Patient name is required")]
    [StringLength(100)]
    [Display(Name = "Patient Name")]
    public string PatientName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please choose a date")]
    [DataType(DataType.Date)]
    public DateTime? Date { get; set; }

    [Required(ErrorMessage = "Please choose a time")]
    public string? Time { get; set; }

    public List<string> TimeSlots { get; set; } = new();
}
