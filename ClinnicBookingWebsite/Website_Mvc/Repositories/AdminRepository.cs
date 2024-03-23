using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Website_Mvc.Models;

namespace Website_Mvc.Repositories
{

    public class AdminRepository
    {
		private readonly ClinicBookingProjectContext _context;

		public AdminRepository(ClinicBookingProjectContext context)
		{
			_context = context;
		}

		public List<Account> GetAccList()
		{
			var AccList = _context.Accounts.ToList();
			if(AccList != null)
			{
				return AccList;
			}

			return null;
		}

		public List<Appointment> GetAppointmentList()
		{
			var appList = _context.Appointments.Include(a => a.IdDoctorNavigation).ThenInclude(a => a.AccountInfo)
				.Include(a => a.IdPatientNavigation).ThenInclude(a => a.AccountInfo)
				.Include(a => a.IdDiseaseNavigation)
				.Include(x => x.StatusNavigation)
				.ToList();

			if(appList != null)
			{
				return appList;
			}

			return null;
		}

		public List<Department> GetDepartmentList()
		{
			var departmentList = _context.Departments.ToList();

			if (departmentList != null)
			{
				return departmentList;
			}
			return null;
		}

		public List<AccountInfo> GetDoctorList()
		{
			// Lấy danh sách các IdUser trong bảng Account có Roll là 2
			var doctorIds = _context.Accounts.Where(a => a.Roll == 2).Select(a => a.IdUser).ToList();

			// Lấy danh sách các bản ghi trong bảng AccountInfo với điều kiện IdUser là các IdUser thuộc danh sách trên
			var doctorInfoList = _context.AccountInfos.Where(ai => doctorIds.Contains(ai.IdUser)).ToList();

			if (doctorInfoList != null)
			{
				return doctorInfoList;
			}
			return null;
		}

		public List<AccountInfo> GetPatientList()
		{
			// Lấy danh sách các IdUser trong bảng Account có Roll là 3
			var PatientIds = _context.Accounts.Where(a => a.Roll == 3).Select(a => a.IdUser).ToList();

			// Lấy danh sách các bản ghi trong bảng AccountInfo với điều kiện IdUser là các IdUser thuộc danh sách trên
			var patientInfoList = _context.AccountInfos.Where(ai => PatientIds.Contains(ai.IdUser)).ToList();

			if (patientInfoList != null)
			{
				return patientInfoList;
			}
			return null;
		}

		public void GetReport(HttpContext httpContext)
		{
			

			var patientNum = _context.Accounts.Where(a => a.Roll == 3).Count();

			var doctorNum = _context.Accounts.Where(a => a.Roll == 2).Count();

			var appointmentNum = _context.Appointments.Select(a => a.IdAppointment).Count();

			var totalRevenue = _context.Appointments.Select(a => a.TotalPrice).Sum();

			httpContext.Session.SetString("patientNum", patientNum.ToString());
			httpContext.Session.SetString("doctorNum", doctorNum.ToString());
			httpContext.Session.SetString("appointmentNum", appointmentNum.ToString());
			httpContext.Session.SetString("totalRevenue", totalRevenue.ToString());
		}

		public void ChangeStatus(string currentStatus, string accountId)
		{
			var accountToUpdate = _context.Accounts.FirstOrDefault(a => a.IdUser == int.Parse(accountId));

			if(currentStatus == "1")
			{
				accountToUpdate.AccountStatus = 0;
				_context.Accounts.Update(accountToUpdate);
				_context.SaveChanges();
			}

			else
			{
				accountToUpdate.AccountStatus = 1;
				_context.Accounts.Update(accountToUpdate);
				_context.SaveChanges();
			}
		}
	}
}
