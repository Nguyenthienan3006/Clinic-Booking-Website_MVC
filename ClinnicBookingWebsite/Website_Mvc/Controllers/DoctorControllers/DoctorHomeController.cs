using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Mvc.Models;
using Website_Mvc.Repositories;

namespace Website_Mvc.Controllers.DoctorControllers
{
	public class PatientInfo
	{
		public int IdAppointment { get; set; }
		public string PatientName { get; set; }
		public int PatientId { get; set; }
		public int DoctorId { get; set; }
	}
	public class DoctorHomeController : Controller
	{
		private readonly DoctorRepository _doctorRepository;
		private readonly ILogger<DoctorHomeController> _logger;

		public DoctorHomeController(DoctorRepository doctorRepository, ILogger<DoctorHomeController> logger)
		{
			_doctorRepository = doctorRepository;
			_logger = logger;
		}

		public IActionResult Index()
		{
			var userId = int.TryParse(HttpContext.Session.GetString("UserId"), out int result) ? result : 0;
			var doctor = _doctorRepository.GetDoctorProfileById(userId);
			var doctorAppointments = _doctorRepository.GetAppointmentList(userId);
			var doctorTodayAppointments = _doctorRepository.GetAppointmentTodayList(userId);
			var doctorStatistics = _doctorRepository.GetDoctorStatistics(userId);
			var doctorFeedbacks = _doctorRepository.GetDoctorFeedbacks(userId);
			var doctorReviews = _doctorRepository.GetDoctorReviews(userId);

			var viewModel = new DoctorViewModel
			{
				Doctor = doctor,
				GetAppointmentList = doctorAppointments,
				DoctorTodayAppointments = doctorTodayAppointments,
				PatientCount = doctorStatistics.patientCount,
				TotalAppointmentCount = doctorStatistics.totalAppointmentCount,
				TodayAppointmentCount = doctorStatistics.todayAppointmentCount,
				GetDoctorFeedbacks = doctorFeedbacks,
				GetDoctorReviews = doctorReviews,
			};

			return View(viewModel);
		}
		public IActionResult AcceptAppointment(int appointmentId)
		{
			// Gọi hàm từ DoctorRepository để cập nhật trạng thái của cuộc hẹn
			_doctorRepository.AcceptAppointmentStatus(appointmentId); // Chuyển trạng thái thành 2 (Accept)

			// Redirect hoặc trả về view tùy thuộc vào logic của bạn
			return RedirectToAction("Index", "DoctorHome");
		}
		public IActionResult CancelAppointment(int appointmentId)
		{
			// Gọi hàm từ DoctorRepository để cập nhật trạng thái của cuộc hẹn
			_doctorRepository.CancelAppointmentStatus(appointmentId); // Chuyển trạng thái thành 2 (Accept)

			// Redirect hoặc trả về view tùy thuộc vào logic của bạn
			return RedirectToAction("Index", "DoctorHome");
		}
		public IActionResult WriteFeedback(int appointmentId)
		{
			var patientInfo = _doctorRepository.GetPatientInfoByAppointmentId(appointmentId);
			ViewBag.patientInfo = patientInfo;
			return View();
		}


		public IActionResult FeedbackPatient(int IdAppointment, int DoctorId, int PatientId, string FeedbackContent)
		{
			_doctorRepository.CompleteAppointmentStatus(IdAppointment);
			// Tạo một feedback mới
			var newFeedback = new DoctorFeedbacksPatient
			{
				IdAppointment = IdAppointment,
				IdDoctor = DoctorId,
				IdPatient = PatientId,
				FeedbackContent = FeedbackContent
			};

			// Thêm feedback mới vào cơ sở dữ liệu
			_doctorRepository.AddFeedback(newFeedback);
			TempData["FeedbackSuccessfulMessage"] = "Feedback Successful!";
			// Chuyển hướng người dùng đến trang chính hoặc trang khác tùy thuộc vào logic của bạn
			return RedirectToAction("Index", "DoctorHome");
		}


	}
}