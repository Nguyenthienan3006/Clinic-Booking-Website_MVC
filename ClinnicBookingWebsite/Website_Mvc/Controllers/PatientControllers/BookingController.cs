using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Website_Mvc.Repositories;

namespace Website_Mvc.Controllers.PatientControllers
{
    public class BookingController : Controller
    {
        private readonly PatientRepository _repository;

        public BookingController(PatientRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index(int id)
        {
            
            List<String> listDiseases = _repository.getDiseases(id);
            var DoctorInfo = _repository.GetDoctorById(id);
            if (ViewBag.ListDiseases == null)
            {
                ViewBag.ListDiseases = new SelectList(listDiseases);
            }

            if (DoctorInfo != null)
            {
                ViewBag.DoctorInfo = DoctorInfo;
            }

            return View();
        }

        public IActionResult CheckOut(string date, string time, string selectedDisease, string description)
        {
            var patientId = HttpContext.Session.GetString("UserId");

            //var patientInfo = _repository.getPatientInfo(int.Parse(patientId));


            return View();
        }
    }
}
