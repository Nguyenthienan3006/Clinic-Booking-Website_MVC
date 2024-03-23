namespace Website_Mvc.Models;

public class AppointmentFeedbackViewModel
{
	public List<Appointment> Appointments { get; set; }
	public List<Appointment> AppointmentsCompleted { get; set; }
	public List<DoctorFeedbacksPatient> DoctorFeedbacks { get; set; }
	public AccountInfo AccountInfo { get; set; }
	public List<PatientReviewsDoctor> PatientReviews { get; set; }
	public List<Appointment> AppointmentUpcoming { get; set; }
}

