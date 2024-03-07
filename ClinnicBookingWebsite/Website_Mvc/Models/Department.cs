using System;
using System.Collections.Generic;

namespace Website_Mvc.Models
{
    public partial class Department
    {
        public Department()
        {
            Diseases = new HashSet<Disease>();
        }

        public string DepartmentId { get; set; } = null!;
        public string? DepartmentName { get; set; }
        public string? Department_Image { get; set; }


        public virtual DoctorDepartment? DoctorDepartment { get; set; }
        public virtual ICollection<Disease> Diseases { get; set; }
    }
}
