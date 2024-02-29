using System;
using System.Collections.Generic;

namespace Website_Mvc.Models
{
    public partial class AppointmentStatus
    {
        public AppointmentStatus()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int IdStatus { get; set; }
        public string? StatusName { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
