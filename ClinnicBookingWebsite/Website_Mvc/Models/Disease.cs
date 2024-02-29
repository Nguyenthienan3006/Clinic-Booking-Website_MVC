using System;
using System.Collections.Generic;

namespace Website_Mvc.Models
{
    public partial class Disease
    {
        public Disease()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int IdDisease { get; set; }
        public string? IdDepartment { get; set; }
        public string? DiseaseName { get; set; }
        public decimal? Price { get; set; }

        public virtual Department? IdDepartmentNavigation { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
