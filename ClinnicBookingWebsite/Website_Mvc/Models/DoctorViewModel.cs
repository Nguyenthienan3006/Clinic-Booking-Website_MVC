namespace Website_Mvc.Models
{
	public class DoctorViewModel
	{
		public (int IdUser, string FullName, string Address, string DepartmentName, string DiseaseName, string UserImage, string DepartmentImage, int ReviewCount) Doctor { get; set; }
		public IEnumerable<(int IdAppointment, int IdPatient, DateTime? AppointmentDate, TimeSpan? AppointmentTime, decimal? TotalPrice, string PatientFirstName, string PatientLastName, string DiseaseName, string Description, string StatusName)> GetAppointmentList { get; set; }
		public IEnumerable<(int IdAppointment, int IdPatient, DateTime? AppointmentDate, TimeSpan? AppointmentTime, decimal? TotalPrice, string PatientFirstName, string PatientLastName, string DiseaseName, string Description, string StatusName)> DoctorTodayAppointments { get; set; }
		public int PatientCount { get; set; }
		public int TotalAppointmentCount { get; set; }
		public int TodayAppointmentCount { get; set; }
		public IEnumerable<(int IdAppointment, string PatientName, string DiseaseName, DateTime? AppointmentDate, string FeedbackContent)> GetDoctorFeedbacks { get; set; }
		public IEnumerable<(int IdReviews, string PatientName, string Email, string ReviewContent)> GetDoctorReviews { get; set; }
	}
}