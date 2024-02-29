using System;
using System.Collections.Generic;

namespace Website_Mvc.Models
{
    public partial class DoctorDepartment
    {
        public string DepartmentId { get; set; } = null!;
        public int? IdUser { get; set; }

        public virtual Department Department { get; set; } = null!;
        public virtual Account? IdUserNavigation { get; set; }
    }
}
