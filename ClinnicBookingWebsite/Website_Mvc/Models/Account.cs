using System;
using System.Collections.Generic;

namespace Website_Mvc.Models
{
    public partial class Account
    {
        public Account()
        {
            AppointmentIdDoctorNavigations = new HashSet<Appointment>();
            AppointmentIdPatientNavigations = new HashSet<Appointment>();
            DoctorDepartments = new HashSet<DoctorDepartment>();
            DoctorFeedbacksPatientIdDoctorNavigations = new HashSet<DoctorFeedbacksPatient>();
            DoctorFeedbacksPatientIdPatientNavigations = new HashSet<DoctorFeedbacksPatient>();
            PatientReviewsDoctors = new HashSet<PatientReviewsDoctor>();
        }

        public int IdUser { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? Roll { get; set; }

        public virtual Roll? RollNavigation { get; set; }
        public virtual AccountInfo? AccountInfo { get; set; }
        public virtual ICollection<Appointment> AppointmentIdDoctorNavigations { get; set; }
        public virtual ICollection<Appointment> AppointmentIdPatientNavigations { get; set; }
        public virtual ICollection<DoctorDepartment> DoctorDepartments { get; set; }
        public virtual ICollection<DoctorFeedbacksPatient> DoctorFeedbacksPatientIdDoctorNavigations { get; set; }
        public virtual ICollection<DoctorFeedbacksPatient> DoctorFeedbacksPatientIdPatientNavigations { get; set; }
        public virtual ICollection<PatientReviewsDoctor> PatientReviewsDoctors { get; set; }
    }
}
