namespace HospitalSystem.Models;

public class Appointment
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
}
