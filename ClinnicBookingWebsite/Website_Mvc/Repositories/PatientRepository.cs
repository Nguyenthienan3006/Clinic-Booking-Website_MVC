using Website_Mvc.Models;

namespace Website_Mvc.Repositories
{
    public class PatientRepository
    {
        private readonly ClinicBookingProjectContext _context;

        public PatientRepository(ClinicBookingProjectContext context)
        {
            _context = context;
        }

        public AccountInfo getPatientInfo(int id)
        {
            var patient = _context.AccountInfos.FirstOrDefault(a => a.IdUser == id);
            if (patient != null)
            {
                return patient;
            }

            return null;
        }

        public List<String> getDiseases(int doctorId)
        {
            List<String> diseases = new List<String>();

            var doctorDepartment = _context.DoctorDepartments.FirstOrDefault(d => d.IdUser == doctorId);

            // Kiểm tra xem doctorDepartment có tồn tại không
            if (doctorDepartment != null)
            {
                // Lấy department_Id từ doctorDepartment
                string departmentId = doctorDepartment.DepartmentId;
                // Lấy danh sách bệnh thuộc phòng của bác sĩ
                var departmentDiseases = _context.Diseases
                    .Where(d => d.IdDepartment.Equals(departmentId))
                    .ToList();

                // Thêm tên bệnh vào danh sách diseases
                diseases.AddRange(departmentDiseases.Select(d => d.DiseaseName));

            }

            return diseases;
        }
    }
}
