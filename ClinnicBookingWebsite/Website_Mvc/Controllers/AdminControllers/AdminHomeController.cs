using Microsoft.AspNetCore.Mvc;
using Website_Mvc.Repositories;

namespace Website_Mvc.Controllers
{
	public class AdminHomeController : Controller
	{
		private readonly AdminRepository _repository;

		public AdminHomeController(AdminRepository repository)
		{
			_repository = repository;
		}
		public IActionResult Index()
		{
			_repository.GetReport(HttpContext);
			var accountList = _repository.GetAccList();
			var departmentList = _repository.GetDepartmentList();
			var doctorList = _repository.GetDoctorList();
			var patientList = _repository.GetPatientList();
			var appointmentList = _repository.GetAppointmentList();

			if(doctorList != null)
			{
				ViewBag.DoctorList = doctorList;
			}
			if(patientList != null)
			{
				ViewBag.PatientList = patientList;
			}
			if(accountList != null)
			{
				ViewBag.AccountList = accountList;
			}
			if(departmentList != null)
			{
				ViewBag.DepartmentList = departmentList;
			}
			if(appointmentList != null)
			{
				ViewBag.AppointmentList = appointmentList;
			}
			return View();
		}

		public IActionResult ChangeStatus(string currentStatus, string accountId)
		{
			_repository.ChangeStatus(currentStatus, accountId);

			return RedirectToAction("Index");
		}
	}
}
