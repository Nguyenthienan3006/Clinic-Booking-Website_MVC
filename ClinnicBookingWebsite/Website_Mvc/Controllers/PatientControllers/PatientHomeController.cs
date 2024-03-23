using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Mvc.Models;
using Website_Mvc.Repositories;

namespace Website_Mvc.Controllers.PatientControllers
{
	public class PatientHomeController : Controller
	{
		private readonly ClinicBookingProjectContext context;

		public List<Appointment> Appointments { get; set; }
		public List<Appointment> AppointmentsCompleted { get; set; }
		public List<DoctorFeedbacksPatient> Feedbacks { get; set; }
		public List<PatientReviewsDoctor> Reviews { get; set; }
		public List<Appointment> AppointmentUpcoming { get; set; }


		public PatientHomeController()
		{
			context = new ClinicBookingProjectContext();
		}
		List<Appointment> appointments;

		// Trong AppointmentController
		public IActionResult Index(string txtSearch)
		{
			var session = HttpContext.Session;
			string valueUser = session.GetString("UserId");
			if (txtSearch != null)
			{
				appointments = context.Appointments.Include(x => x.IdDoctorNavigation).ThenInclude(d => d.AccountInfo).
				Include(x => x.IdDiseaseNavigation).Include(x => x.StatusNavigation).
				Where(x => x.IdPatient == int.Parse(valueUser) && x.IdDiseaseNavigation.DiseaseName.Contains(txtSearch)).ToList();
			}
			else
			{
				appointments = context.Appointments.Include(x => x.IdDoctorNavigation).ThenInclude(d => d.AccountInfo).
				Include(x => x.IdDiseaseNavigation).Include(x => x.StatusNavigation).
				Where(x => x.IdPatient == int.Parse(valueUser)).ToList();
			}


			var appointmentsCompleted = context.Appointments.Include(x => x.IdDoctorNavigation).ThenInclude(d => d.AccountInfo).Include(x => x.IdDiseaseNavigation).Include(x => x.StatusNavigation).Where(x => x.IdPatient == int.Parse(valueUser) && x.Status == 4).ToList();
			var doctorfeedbacks = context.DoctorFeedbacksPatients.Include(x => x.IdAppointmentNavigation).Include(x => x.IdDoctorNavigation).ThenInclude(d => d.AccountInfo).Where(x => x.IdPatient == int.Parse(valueUser)).ToList();
			var user = context.AccountInfos.Where(x => x.IdUser == int.Parse(valueUser)).FirstOrDefault();
			var reviews = context.PatientReviewsDoctors.Include(x => x.IdDoctorNavigation).ThenInclude(d => d.AccountInfo).Where(x => x.IdPatient == int.Parse(valueUser)).ToList();
			var appoimentUpcoming = context.Appointments.Include(x => x.IdDoctorNavigation).ThenInclude(d => d.AccountInfo).Include(x => x.IdDiseaseNavigation).Include(x => x.StatusNavigation).Where(x => x.IdPatient == int.Parse(valueUser) && x.Status == 2 && x.AppointmentTime >= DateTime.Today).ToList();
			var viewModel = new AppointmentFeedbackViewModel
			{
				Appointments = appointments,
				AppointmentsCompleted = appointmentsCompleted,
				DoctorFeedbacks = doctorfeedbacks,
				AccountInfo = user,
				PatientReviews = reviews,
				AppointmentUpcoming = appoimentUpcoming
			};

			return View(viewModel);
		}
		public IActionResult Delete(int? id)
		{
			var session = HttpContext.Session;
			string valueUser = session.GetString("UserId");
			var doctorfeedbacks = context.DoctorFeedbacksPatients.Where(x => x.IdAppointment == id).FirstOrDefault();
			var appointmentDelete = context.Appointments.Where(x => x.IdAppointment == id && x.IdPatient == int.Parse(valueUser)).FirstOrDefault();
			context.Appointments.Remove(appointmentDelete);
			context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		public IActionResult FeedbackDoctor(int doctorId, int idAppointment)
		{
			var session = HttpContext.Session;
			string valueUser = session.GetString("UserId");

			Account doctor = context.Accounts.Include(x => x.AccountInfo).FirstOrDefault(x => x.IdUser == doctorId);
			var patientReviewDoctorExist = context.PatientReviewsDoctors.Include(x => x.IdAppointmentNavigation).Where(x => x.IdAppointment == idAppointment && x.IdPatient == int.Parse(valueUser)).FirstOrDefault();
			if (patientReviewDoctorExist != null)
			{
				TempData["AlreadyReviewMessage"] = "Already Review!";
				return RedirectToAction(nameof(Index));
			}
			session.SetString("doctorId", doctorId.ToString());
			session.SetString("idAppointment", idAppointment.ToString());
			ViewBag.Doctor = doctor.AccountInfo.FullName;
			return View();
		}
		[HttpPost]
		public IActionResult FeedbackDoctor(string reviewDoctor)
		{
			var session = HttpContext.Session;
			string valueUser = session.GetString("UserId");
			string valueDoctor = session.GetString("doctorId");
			string valueAppointment = session.GetString("idAppointment");
			PatientReviewsDoctor patientReviewsDoctor = new PatientReviewsDoctor();

			patientReviewsDoctor.IdDoctor = int.Parse(valueDoctor);
			patientReviewsDoctor.IdAppointment = int.Parse(valueAppointment);
			patientReviewsDoctor.IdPatient = int.Parse(valueUser);
			patientReviewsDoctor.ReviewContent = reviewDoctor;
			if (patientReviewsDoctor.IdDoctor != null)
			{

				context.Add(patientReviewsDoctor);
				context.SaveChanges();
				TempData["ReviewSuccessfulMessage"] = "Review Successful!";
				return RedirectToAction(nameof(Index));
			}
			return View();

		}
		public IActionResult DeletePatienReview(int idReview)
		{
			var session = HttpContext.Session;
			string valueUser = session.GetString("UserId");
			var patientReview = context.PatientReviewsDoctors.Where(x => x.IdReviews == idReview && x.IdPatient == int.Parse(valueUser)).FirstOrDefault();
			context.PatientReviewsDoctors.Remove(patientReview);
			context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}
	

