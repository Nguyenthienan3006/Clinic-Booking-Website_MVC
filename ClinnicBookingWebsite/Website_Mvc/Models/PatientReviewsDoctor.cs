using System;
using System.Collections.Generic;

namespace Website_Mvc.Models
{
    public partial class PatientReviewsDoctor
    {
		public int IdReviews { get; set; }
		public int? IdAppointment { get; set; }
		public int? IdDoctor { get; set; }
		public int? IdPatient { get; set; }
		public string? ReviewContent { get; set; }

		public virtual Appointment? IdAppointmentNavigation { get; set; }
		public virtual Account? IdDoctorNavigation { get; set; }
		public virtual Account? IdPatientNavigation { get; set; }
	}
}
