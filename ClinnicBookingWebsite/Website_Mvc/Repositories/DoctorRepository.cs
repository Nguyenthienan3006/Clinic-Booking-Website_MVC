using System;
using System.Linq;
using System.Collections.Generic;
using Website_Mvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Website_Mvc.Controllers.DoctorControllers;

namespace Website_Mvc.Repositories
{
	public class DoctorRepository
	{
		private readonly ClinicBookingProjectContext _context;
		private readonly ILogger<DoctorRepository> logger;


		public DoctorRepository(ClinicBookingProjectContext context, ILogger<DoctorRepository> logger)
		{
			_context = context;
			logger = logger;
		}
		public IEnumerable<(int IdAppointment, int IdPatient, DateTime? AppointmentDate, TimeSpan? AppointmentTime, decimal? TotalPrice, string PatientFirstName, string PatientLastName, string DiseaseName, string Description, string StatusName)> GetAppointmentList(int userId)
		{
			var doctorAppointments = _context.Appointments
				.Where(a => a.IdDoctor == userId)
				.Join(_context.AppointmentStatuses,
					a => a.Status,
					s => s.IdStatus,
					(a, s) => new
					{
						IdAppointment = a.IdAppointment, // Thêm cột IdAppointment
						IdPatient = a.IdPatient.HasValue ? a.IdPatient.Value : 0,
						AppointmentDateTime = a.AppointmentTime,
						a.TotalPrice,
						PatientFirstName = a.IdPatientNavigation.AccountInfo.FirstName,
						PatientLastName = a.IdPatientNavigation.AccountInfo.LastName,
						DiseaseName = a.IdDiseaseNavigation.DiseaseName,
						Description = a.PatientConditionDescription,
						s.StatusName
					})
				.ToList()
				.Select(a => (
					IdAppointment: a.IdAppointment, // Thêm IdAppointment vào kết quả
					IdPatient: a.IdPatient,
					AppointmentDate: a.AppointmentDateTime?.Date,
					AppointmentTime: a.AppointmentDateTime?.TimeOfDay,
					TotalPrice: a.TotalPrice,
					PatientFirstName: a.PatientFirstName,
					PatientLastName: a.PatientLastName,
					DiseaseName: a.DiseaseName,
					Description: a.Description,
					StatusName: a.StatusName
				));

			return doctorAppointments;
		}

		public IEnumerable<(int IdAppointment, int IdPatient, DateTime? AppointmentDate, TimeSpan? AppointmentTime, decimal? TotalPrice, string PatientFirstName, string PatientLastName, string DiseaseName, string Description, string StatusName)> GetAppointmentTodayList(int userId)
		{
			var today = DateTime.Today;

			var doctorAppointments = _context.Appointments
				.Where(a => a.IdDoctor == userId && a.AppointmentTime.HasValue && a.AppointmentTime.Value.Date == today)
				.Join(_context.AppointmentStatuses,
					a => a.Status,
					s => s.IdStatus,
					(a, s) => new
					{
						IdAppointment = a.IdAppointment, // Thêm cột IdAppointment
						IdPatient = a.IdPatient.HasValue ? a.IdPatient.Value : 0,
						AppointmentDate = a.AppointmentTime.HasValue ? a.AppointmentTime.Value.Date : (DateTime?)null,
						AppointmentTime = a.AppointmentTime.HasValue ? a.AppointmentTime.Value.TimeOfDay : (TimeSpan?)null,
						TotalPrice = a.TotalPrice,
						PatientFirstName = a.IdPatientNavigation.AccountInfo.FirstName,
						PatientLastName = a.IdPatientNavigation.AccountInfo.LastName,
						DiseaseName = a.IdDiseaseNavigation.DiseaseName,
						Description = a.PatientConditionDescription,
						StatusName = s.StatusName
					})
				.ToList()
				.Select(a => (
					IdAppointment: a.IdAppointment, // Thêm IdAppointment vào kết quả
					IdPatient: a.IdPatient,
					AppointmentDate: a.AppointmentDate,
					AppointmentTime: a.AppointmentTime,
					TotalPrice: a.TotalPrice,
					PatientFirstName: a.PatientFirstName,
					PatientLastName: a.PatientLastName,
					DiseaseName: a.DiseaseName,
					Description: a.Description,
					StatusName: a.StatusName
				));

			return doctorAppointments;
		}





		public (int IdUser, string fullName, string address, string departmentName, string diseaseName, string userImage, string departmentImage, int reviewCount) GetDoctorProfileById(int userId)
		{
			var doctorInfo = _context.Accounts
				.Where(u => u.RollNavigation.RollName == "Doctor" && u.IdUser == userId)
				.Join(_context.AccountInfos, acc => acc.IdUser, accInfo => accInfo.IdUser,
					(acc, accInfo) => new { Account = acc, AccountInfo = accInfo })
				.Join(_context.DoctorDepartments, acc => acc.Account.IdUser, dd => dd.IdUser,
					(acc, dd) => new { Account = acc.Account, AccountInfo = acc.AccountInfo, DoctorDepartment = dd })
				.Join(_context.Departments, dd => dd.DoctorDepartment.DepartmentId, d => d.DepartmentId,
					(dd, d) => new { Account = dd.Account, AccountInfo = dd.AccountInfo, Department = d })
				.Select(dd => new
				{
					dd.AccountInfo.IdUser,
					FullName = $"{dd.AccountInfo.FirstName} {dd.AccountInfo.LastName}",
					dd.AccountInfo.Address,
					dd.AccountInfo.User_Image,
					dd.Department.DepartmentName,
					departmentImage = dd.Department.Department_Image,
					DepartmentId = dd.Department.DepartmentId
				})
				.FirstOrDefault();

			if (doctorInfo != null)
			{
				var reviewCount = _context.PatientReviewsDoctors.Count(pr => pr.IdDoctor == doctorInfo.IdUser);
				var diseaseName = _context.Diseases.FirstOrDefault(d => d.IdDepartment == doctorInfo.DepartmentId)?.DiseaseName;

				return (doctorInfo.IdUser, doctorInfo.FullName, doctorInfo.Address, doctorInfo.DepartmentName, diseaseName, doctorInfo.User_Image, doctorInfo.departmentImage, reviewCount);
			}

			return (0, "", "", "", "", "", "", 0); // Trả về giá trị mặc định nếu không tìm thấy thông tin bác sĩ
		}
		public (int patientCount, int totalAppointmentCount, int todayAppointmentCount) GetDoctorStatistics(int userId)
		{
			DateTime today = DateTime.Today;

			var appointments = _context.Appointments.Where(a => a.IdDoctor == userId);

			int patientCount = appointments.Select(a => a.IdPatient).Distinct().Count();
			int totalAppointmentCount = appointments.Count();
			int todayAppointmentCount = appointments.Count(a => a.AppointmentTime >= today && a.AppointmentTime < today.AddDays(1));

			return (patientCount, totalAppointmentCount, todayAppointmentCount);
		}
		public IEnumerable<(int IdAppointment, string PatientName, string DiseaseName, DateTime? AppointmentDate, string FeedbackContent)> GetDoctorFeedbacks(int doctorId)
		{
			var doctorFeedbacks = _context.DoctorFeedbacksPatients
				.Where(df => df.IdDoctor == doctorId)
				.Select(df => new
				{
					IdAppointment = df.IdAppointment ?? 0,
					PatientName = $"{df.IdPatientNavigation.AccountInfo.FirstName} {df.IdPatientNavigation.AccountInfo.LastName}",
					DiseaseName = df.IdAppointmentNavigation.IdDiseaseNavigation.DiseaseName,
					AppointmentDate = df.IdAppointmentNavigation.AppointmentTime,
					FeedbackContent = df.FeedbackContent
				})
				.ToList()
				.Select(df => (
					IdAppointment: df.IdAppointment,
					PatientName: df.PatientName,
					DiseaseName: df.DiseaseName,
					AppointmentDate: df.AppointmentDate,
					FeedbackContent: df.FeedbackContent
				));

			return doctorFeedbacks;
		}
		public IEnumerable<(int IdReviews, string PatientName, string Email, string ReviewContent)> GetDoctorReviews(int doctorId)
		{
			var reviews = _context.PatientReviewsDoctors
				.Where(pr => pr.IdDoctor == doctorId)
				.Select(pr => new
				{
					IdReviews = pr.IdReviews,
					IdReviewers = pr.IdPatient,
					ReviewContent = pr.ReviewContent
				})
				.ToList()
				.Select(pr => new
				{
					pr.IdReviews,
					PatientName = _context.AccountInfos
						.Where(ai => ai.IdUser == pr.IdReviewers)
						.Select(ai => ai.FirstName + " " + ai.LastName)
						.FirstOrDefault(),
					Email = _context.AccountInfos
						.Where(ai => ai.IdUser == pr.IdReviewers)
						.Select(ai => ai.Email)
						.FirstOrDefault(),
					pr.ReviewContent
				})
				.ToList()
				.Select(pr => (
					IdReviews: pr.IdReviews,
					PatientName: pr.PatientName ?? "", // Xử lý trường hợp không tìm thấy thông tin người đánh giá
					Email: pr.Email ?? "", // Xử lý trường hợp không tìm thấy thông tin email
					ReviewContent: pr.ReviewContent
				));

			return reviews;
		}
		public void AcceptAppointmentStatus(int appointmentId)
		{
			// Tìm cuộc hẹn cần cập nhật trạng thái
			var appointment = _context.Appointments.FirstOrDefault(a => a.IdAppointment == appointmentId);

			if (appointment != null)
			{
				// Cập nhật trạng thái thành "Accept" (2)
				appointment.Status = 2;
				_context.SaveChanges();
			}
			// Xử lý trường hợp không tìm thấy cuộc hẹn hoặc lỗi khác nếu cần
		}

		public void CancelAppointmentStatus(int appointmentId)
		{
			// Tìm cuộc hẹn cần cập nhật trạng thái
			var appointment = _context.Appointments.FirstOrDefault(a => a.IdAppointment == appointmentId);

			if (appointment != null)
			{
				// Cập nhật trạng thái thành "Cancel" (3)
				appointment.Status = 3;
				_context.SaveChanges();
			}
			// Xử lý trường hợp không tìm thấy cuộc hẹn hoặc lỗi khác nếu cần
		}
		public PatientInfo GetPatientInfoByAppointmentId(int appointmentId)
		{
			var appointment = _context.Appointments
				.Include(a => a.IdPatientNavigation.AccountInfo)
				.FirstOrDefault(a => a.IdAppointment == appointmentId);

			if (appointment != null)
			{
				var patientInfo = new PatientInfo
				{
					IdAppointment = appointment.IdAppointment,
					PatientName = $"{appointment.IdPatientNavigation.AccountInfo.FirstName} {appointment.IdPatientNavigation.AccountInfo.LastName}",
					DoctorId = appointment.IdDoctor ?? 0,
					PatientId = appointment.IdPatient ?? 0 // Sử dụng ?? để xác định giá trị mặc định (0) nếu IdPatient là null
				};

				return patientInfo;
			}

			return null;
		}


		public void AddFeedback(DoctorFeedbacksPatient newFeedback)
		{
			// Thêm feedback mới vào DbSet
			_context.DoctorFeedbacksPatients.Add(newFeedback);
			// Lưu thay đổi vào cơ sở dữ liệu
			_context.SaveChanges();
		}
		public void CompleteAppointmentStatus(int appointmentId)
		{
			// Tìm cuộc hẹn cần cập nhật trạng thái
			var appointment = _context.Appointments.FirstOrDefault(a => a.IdAppointment == appointmentId);

			if (appointment != null)
			{
				// Cập nhật trạng thái thành "Cancel" (3)
				appointment.Status = 4;
				_context.SaveChanges();
			}
			// Xử lý trường hợp không tìm thấy cuộc hẹn hoặc lỗi khác nếu cần
		}

	}
}