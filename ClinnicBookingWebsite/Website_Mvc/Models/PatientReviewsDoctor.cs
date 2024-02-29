using System;
using System.Collections.Generic;

namespace Website_Mvc.Models
{
    public partial class PatientReviewsDoctor
    {
        public int IdReviews { get; set; }
        public int? IdReviewers { get; set; }
        public int? IdDoctor { get; set; }
        public string? ReviewContent { get; set; }

        public virtual Account? IdDoctorNavigation { get; set; }
    }
}
