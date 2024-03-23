using System;
using System.Collections.Generic;

namespace Website_Mvc.Models
{
    public partial class Appointment
    {
		public int IdAppointment { get; set; }
		public int? IdDoctor { get; set; }
		public int? IdPatient { get; set; }
		public int? IdDisease { get; set; }
		public string? PatientConditionDescription { get; set; }
		public DateTime? AppointmentTime { get; set; }
		public int? Status { get; set; }
		public decimal? TotalPrice { get; set; }

		public virtual Disease? IdDiseaseNavigation { get; set; }
		public virtual Account? IdDoctorNavigation { get; set; }
		public virtual Account? IdPatientNavigation { get; set; }
		public virtual AppointmentStatus? StatusNavigation { get; set; }
		public virtual ICollection<DoctorFeedbacksPatient> DoctorFeedbacksPatients { get; set; }
		public virtual ICollection<PatientReviewsDoctor> PatientReviewsDoctors { get; set; }
	}
}
