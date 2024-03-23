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
                var departmentDiseases = _context.Diseases.Where(d => d.IdDepartment == departmentId);

                // Thêm tên bệnh vào danh sách diseases
                diseases.AddRange(departmentDiseases.Select(d => d.DiseaseName));


                return diseases;

            }

            return null;
        }

        public AccountInfo GetDoctorById(int id)
        {
            AccountInfo accountInfo = _context.AccountInfos.FirstOrDefault(a => a.IdUser == id);

            if(accountInfo != null)
            {
                return accountInfo;
            }

            return null;
        }

        public void AddAppointment(HttpContext context)
        {
            
            //lấy Id disease

            var bookingDisease = context.Session.GetString("BookingDisease");
            var disease = _context.Diseases.FirstOrDefault(d => d.DiseaseName == bookingDisease);



            // Lấy các giá trị từ Session
            var bookingDate = context.Session.GetString("BookingDate");
            var bookingTime = context.Session.GetString("BookingTime");


            var bookingDescription = context.Session.GetString("BookingDescription");
            var DoctorName = context.Session.GetString("DoctorName");
            var DoctorAddress = context.Session.GetString("DoctorAddress");
            var DoctorId = context.Session.GetString("DoctorId");

            var diseaseId = disease.IdDisease;

            string patientId = context.Session.GetString("UserId");
            string totalPrice = context.Session.GetString("totalPrice");


            var dateOfMonth = bookingDate;
            var selectedTimeText = bookingTime;


            // Kết hợp thông tin để có ngày đầy đủ
            var dateTimeInfo = dateOfMonth + ' ' + selectedTimeText;

            // Thêm 7 giờ vào thời gian đã chọn
            var selectedDateTime = DateTime.Parse(dateTimeInfo);
            // Format selectedDateTime as "2024-03-05 16:30:00.000"
            var formattedDateTime = DateTime.Parse(selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));



            Appointment appointment = new Appointment
            {
                IdDoctor = int.Parse(DoctorId),
                IdPatient = int.Parse(patientId),
                IdDisease = diseaseId,
                PatientConditionDescription = bookingDescription,
                AppointmentTime = formattedDateTime,
                Status = 1,
                TotalPrice = Convert.ToDecimal(totalPrice)
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();


            
        }

        public bool ClearSession(HttpContext context)
        {

            context.Session.Remove("DoctorId");
            context.Session.Remove("BookingDescription");
            context.Session.Remove("BookingDate");
            context.Session.Remove("BookingTime");
            context.Session.Remove("DoctorName");
            context.Session.Remove("DoctorAddress");
            context.Session.Remove("totalPrice");
            context.Session.Remove("BookingDisease");
            context.Session.Remove("DiseasePrice");
            return true;
        }

        public Disease getDiseasesPrice(string selectedDisease)
        {
            var disease = _context.Diseases.FirstOrDefault(d => d.DiseaseName == selectedDisease);

            if(disease != null ) 
            {
                return disease;
            }

            return null;
        }
    }
}
